using AdminShared;
using AdminManager.Accessors;
using AdminManager.Common;
using AdminManager.Job;
using AdminManager.Model;
using AdminManager.ModelView;
using Quartz;

namespace AdminManager.Service;

public class EventService(IServiceProvider sp)
{
    public event Action OnAirdropEventComplete;

    private const string JobKey = "EventAirdrop";

    private readonly IDBAccessor _dbAccessor = sp.GetRequiredService<IDBAccessor>();
    private readonly IMapperService _mapService = sp.GetRequiredService<IMapperService>();
    private readonly IRedisService _redisService = sp.GetRequiredService<IRedisService>();
    private readonly ScheduleService _scheduleService = sp.GetRequiredService<ScheduleService>();

    private readonly Dictionary<int, AirdropEventModel> _airdropEvent = new();

    public void Load() => Task.Run(InitAsync).Wait();

    private async Task InitAsync()
    {
        _airdropEvent.Clear();
        foreach (var aem in await GetAirdropEventFromDbAsync())
        {
            if (!_airdropEvent.TryAdd(aem.No, aem))
            {
                continue;
            }

            if (aem.ExecuteRet is EventResultType.Waiting)
            {
                await AddScheduleAsync(aem.No);
            }
        }

        _redisService.Stub.Res_Airdrop = (res) =>
        {
            OnResponseAirdropEvent(res.result, res.eventNo);
            return true;
        };

    }

    public IEnumerable<AirdropEventModelView> GetAirdropEvent()
    {
        if (_airdropEvent.Count == 0)
        {
            Load();
        }

        var airdropEventDesc = _airdropEvent.Values.OrderByDescending(x => x.No).ToList();
        return ToAirdropEventModelView(airdropEventDesc);
    }
    
    public AirdropEventModelView GetAirdropEvent(int no)
    {
        var aem = _airdropEvent.GetValueOrDefault(no);
        if (aem == null)
        {
            return null;
        }

        return ToAirdropEventModelView(aem);
    }

    public async Task<long> GetAirdropAmount(GameType gameType)
    {
        var masterServerId = await _redisService.GetMasterServerIdAsync();
        if (masterServerId <= 0)
        {
            return 0;
        }

        if (GameType.Event == gameType)
        {
            return 0;
        }

        return await _dbAccessor.GetAirdropCollectAmount(masterServerId, gameType);
    }

    public async Task<bool> AddAirdropEventAsync(AirdropEventModelView aemv)
    {
        if (Utility.IsNullOrEmpty(aemv))
        {
            return false;
        }

        if (IsExistAirdropEvent(aemv.No))
        {
            return await UpdateAirdropEventAsync(aemv);
        }

        if (aemv.No == 0)
        {
            aemv.No = _airdropEvent.Count == 0 ? 1 : _airdropEvent.Keys.Max() + 1;
        }

        var aem = ToAirdropEventModel(aemv);
        if (aem is null)
        {
            return false;
        }

        if (!await SetAirdropEventToDbAsync(aem))
        {
            return false;
        }

        if (!_airdropEvent.TryAdd(aem.No, aem))
        {
            return false;
        }

        await AddScheduleAsync(aem.No);

        return true;
    }
    
    private async Task<bool> UpdateAirdropEventAsync(AirdropEventModelView aemv)
    {
        if (aemv is null || aemv.No == 0)
        {
            return false;
        }

        if (!_airdropEvent.ContainsKey(aemv.No))
        {
            return false;
        }

        var aem = ToAirdropEventModel(aemv);
        if (aem is null)
        {
            return false;
        }

        if (!await SetAirdropEventToDbAsync(aem))
        {
            return false;
        }

        _airdropEvent[aem.No] = aem;

        await AddScheduleAsync(aem.No);

        return false;
    }

    public async Task<bool> RemoveAirdropEventAsync(int no)
    {
        if (no == 0)
        {
            return false;
        }

        if (!_airdropEvent.ContainsKey(no))
        {
            return false;
        }

        if (!await RemoveAirdropEventFromDbAsync(no))
        {
            return false;
        }

        _airdropEvent.Remove(no);

        await DeleteScheduleAsync(no);

        return true;
    }

    private async Task<bool> AddScheduleAsync(int no)
    {
        if (!_airdropEvent.TryGetValue(no, out var aem))
        {
            return false;
        }

        if (Utility.IsNullOrEmpty(aem))
        {
            return false;
        }

        ScheduleConfig sc = new()
        {
            BeginDateTime = DateTimeOffset.FromUnixTimeSeconds(aem.BeginTimeStamp).UtcDateTime.ToTimeZone(),
            EndDateTime = DateTime.Now.AddDays(365),
            IntervalSeconds = 60,
            RepeatCount = 1,
            IsImmediate = false
        };
        
        var jobData = new JobDataMap();
        jobData.Put(JobKey, aem);

        var identity = new[] { JobKey, no.ToString() };
        await _scheduleService.CreateScheduleJobAsync<AirdropJob>(identity, jobData, sc);

        // 스케쥴 등록 완료 - 대기 상태
        SetResultAirdropEvent(aem, EventResultType.Waiting);

        return true;
    }

    private async Task DeleteScheduleAsync(int no)
    {
        await _scheduleService.DeleteJob(no, JobKey);
    }

    public async Task<string> GetSchedulerJob() => await _scheduleService.GetJobsAndTriggers(JobKey);

    public void SetCompleteAirdropEvent(int eventNo, EventResultType ret)
    {
        if (!_airdropEvent.TryGetValue(eventNo, out var aem))
        {
            return;
        }

        SetResultAirdropEvent(aem, ret);

        Task.Run(async () =>
        {
            var msg = ret.GetDescription();
            await _dbAccessor.SetAirdropEventResultAsync(aem.No, ret, msg);
            await DeleteScheduleAsync(eventNo);
        });
    }

    private void OnResponseAirdropEvent(AdminShared.Result ret, int eventNo)
    {
        if (_airdropEvent.TryGetValue(eventNo, out var aem))
        {
            var r = (ret == Result.Success) ? EventResultType.Success : EventResultType.Fail;
            SetResultAirdropEvent(aem, r);
            OnAirdropEventComplete?.Invoke();
            return;
        }

        Task.Run(async () =>
        {
            var r = (ret == Result.Success) ? EventResultType.Success : EventResultType.Fail;
            await SetAirdropEventResultToDbAsync(eventNo, r, ret.GetDescription());
        });
    }

    private void SetResultAirdropEvent(AirdropEventModel aem, EventResultType ret)
    {
        aem.ExecuteRet = ret;
        aem.ExecuteMsg = ret.GetDescription();
        aem.ExecuteTimeStamp = Utility.GetTimeStamp();

        Task.Run(async () =>
        {
            await SetAirdropEventResultToDbAsync(aem.No, aem.ExecuteRet, aem.ExecuteMsg);
        });
    }

    public bool IsExistAirdropEvent(int no) => no != 0 && _airdropEvent.ContainsKey(no);
    
    private async Task<List<AirdropEventModel>> GetAirdropEventFromDbAsync() => await _dbAccessor.GetAirdropEventAsync();
    private async Task<bool> SetAirdropEventToDbAsync(AirdropEventModel aes) => aes != null && await _dbAccessor.AddAirdropEventAsync(aes);
    private async Task SetAirdropEventResultToDbAsync(int no, EventResultType ret, string msg) => await _dbAccessor.SetAirdropEventResultAsync(no, ret, msg);
    private async Task<bool> RemoveAirdropEventFromDbAsync(int no) => await _dbAccessor.RemoveAirdropEventAsync(no);

    private AirdropEventModel ToAirdropEventModel(AirdropEventModelView aemv) => _mapService.Map<AirdropEventModelView, AirdropEventModel>(aemv);
    private AirdropEventModelView ToAirdropEventModelView(AirdropEventModel aem) => _mapService.Map<AirdropEventModel, AirdropEventModelView>(aem);
    private List<AirdropEventModelView> ToAirdropEventModelView(List<AirdropEventModel> aem) => _mapService.Map<List<AirdropEventModel>, List<AirdropEventModelView>>(aem);

}
