using Quartz;
using AdminManager.Accessors;
using AdminManager.Common;
using AdminManager.Job;
using AdminManager.Model;
using AdminManager.ModelView;

namespace AdminManager.Service;
public class MessageService(IServiceProvider serviceProvider)
{
    private const string JobKey = "SystemMessage";

    private readonly IDBAccessor _dbAccessor = serviceProvider.GetRequiredService<IDBAccessor>();
    private readonly IMapperService _mapService = serviceProvider.GetRequiredService<IMapperService>();
    private readonly ScheduleService _scheduleService = serviceProvider.GetRequiredService<ScheduleService>();

    private readonly Dictionary<int, SystemMessageModel> _systemMessage = new();
    private readonly Dictionary<int, List<MessageModel>> _message = new();

    public void Load() => Task.Run(InitAsync).Wait();

    private async Task InitAsync()
    {
        _message.Clear();
        _systemMessage.Clear();
        
        foreach (var ms in await GetSystemMessageFromDBAsync())
        {
            ms.MessageType = MessageType.Schedule;

            if (!_systemMessage.TryAdd(ms.No, ms))
            {
                continue;
            }

            var messages = await GetMessageFromDBAsync(ms.No);
            if (messages.Count == 0)
            {
                messages.Add(new MessageModel { No = ms.No, Language = GDT.LanguageType.LT_Korean, Message = string.Empty });
                _message[ms.No] = messages;
                continue;
            }

            _message[ms.No] = messages;

            if (DateTime.Now.IsInRange(ms.BeginTimeStamp, ms.EndTimeStamp))
            {
                await AddScheduleAsync(ms.No);
            }
        }
    }

    public SystemMessageModelView GetSystemMessage(int no)
    {
        var msm = _systemMessage.GetValueOrDefault(no);
        if (msm == null)
        {
            return null;
        }

        return ToSystemMessageModelView(msm);
    }

    public List<SystemMessageModelView> GetSystemMessage()
    {
        var descList = _systemMessage.Values.OrderByDescending(x => x.No).ToList();
        return ToSystemMessageModelView(descList);
    }

    public List<MessageModelView> GetMessage(int no)
    {
        var mm = _message.GetValueOrDefault(no);
        if (mm == null)
        {
            return null;
        }

        return ToMessageModelView(mm);
    }

    public bool IsExistSystemMessage(int no) => no != 0 && _systemMessage.ContainsKey(no);

    public async Task<bool> AddMessageAsync(SystemMessageModelView smv, List<MessageModelView> mv)
    {
        if (Utility.IsNullOrEmpty(smv, mv))
        {
            return false;
        }

        if (IsExistSystemMessage(smv.No))
        {
            return await UpdateMessageAsync(smv, mv);
        }

        if (smv.No == 0)
        {
            smv.No = _systemMessage.Count == 0 ? 1 : _systemMessage.Keys.Max() + 1;
        }

        var sm = ToSystemMessageModel(smv);
        var m = ToMessageModel(mv);
        if (Utility.IsNullOrEmpty(sm) || Utility.IsNullOrEmpty(m))
        {
            return false;
        }

        if (!await AddToDBAsync(sm, m))
        {
            return false;
        }

        if (!_systemMessage.TryAdd(sm.No, sm) || !_message.TryAdd(sm.No, m))
        {
            _systemMessage.Remove(sm.No);
            return false;
        }

        return await AddScheduleAsync(sm.No);
    }

    private async Task<bool> UpdateMessageAsync(SystemMessageModelView smv, List<MessageModelView> mv)
    {
        if (Utility.IsNullOrEmpty(smv, mv))
        {
            return false;
        }

        if (!_systemMessage.ContainsKey(smv.No))
        {
            return false;
        }

        var sm = ToSystemMessageModel(smv);
        var m = ToMessageModel(mv);
        if (Utility.IsNullOrEmpty(sm) || Utility.IsNullOrEmpty(m))
        {
            return false;
        }

        _systemMessage[smv.No] = sm;
        _message[smv.No] = m;

        if (!await AddToDBAsync(sm, m))
        {
            return false;
        }

        return await AddScheduleAsync(smv.No);
    }

    public async Task<bool> DeleteSystemMessageAsync(int no)
    {
        if (!_systemMessage.ContainsKey(no))
        {
            return false;
        }

        _systemMessage.Remove(no);
        _message.Remove(no);

        await _dbAccessor.RemoveMessageScheduleAsync(no);
        await _dbAccessor.RemoveMessageAsync(no);

        await DeleteScheduleTriggerAsync(no);

        return true;
    }

    private bool TryGetMessage(int no, out SystemMessageModel scm, out List<MessageModel> mm)
    {
        //scm = null;
        mm = null;

        if (!_systemMessage.TryGetValue(no, out scm))
        {
            return false;
        }

        if (!_message.TryGetValue(no, out mm))
        {
            return false;
        }

        return true;
    }


    #region scheduler
    private async Task<bool> AddScheduleAsync(int no)
    {
        if (!TryGetMessage(no, out var sm, out var msg))
        {
            return false;
        }

        if (Utility.IsNullOrEmpty(sm, msg))
        {
            return false;
        }

        ScheduleConfig sc = new()
        {
            BeginDateTime = sm.BeginTimeStamp.ToDateTime().ToTimeZone(),
            EndDateTime = sm.EndTimeStamp.ToDateTime().ToTimeZone(),
            IntervalSeconds = (int)sm.Interval * 60,
            RepeatCount = 0,
            IsImmediate = sm.MessageType == MessageType.Instantly
        };

        var jobData = new JobDataMap();
        for (var i = 0; i < msg.Count; i++)
        {
            jobData.Put($"{JobKey}{i}", msg[i]);
        }

        var identity = new[] { JobKey, no.ToString() };
        await _scheduleService.CreateScheduleJobAsync<MessageJob>(identity, jobData, sc);

        return true;
    }
    private async Task DeleteScheduleTriggerAsync(int no)
    {
        await _scheduleService.DeleteJob(no, JobKey);
    }
    public async Task<string> GetSchedulerJob() => await _scheduleService.GetJobsAndTriggers(JobKey);
    #endregion

    #region database access
    private async Task<List<SystemMessageModel>> GetSystemMessageFromDBAsync() => await _dbAccessor.GetSystemMessageAsync();
    private async Task<List<MessageModel>> GetMessageFromDBAsync(int no) => await _dbAccessor.GetMessageAsync(no);
    private async Task<bool> AddToDBAsync(SystemMessageModel msm, List<MessageModel> mm)
    {
        if (Utility.IsNullOrEmpty(msm, mm))
        {
            return false;
        }

        if (!await AddSystemMessageToDBAsync(msm))
        {
            _systemMessage.Remove(msm.No);
            _message.Remove(msm.No);
            return false;
        }

        var saveTasks = mm.Select(AddMessageToDBAsync);
        var saveResults = await Task.WhenAll(saveTasks);

        if (!saveResults.All(result => result))
        {
            return false;
        }

        return true;
    }
    private async Task<bool> AddSystemMessageToDBAsync(SystemMessageModel msm)
    {
        if (msm is null)
        {
            return false;
        }

        return await _dbAccessor.AddSystemMessageAsync(msm);
    }
    private async Task<bool> AddMessageToDBAsync(MessageModel mm)
    {
        if (mm is null)
        {
            return false;
        }

        return await _dbAccessor.AddMessageAsync(mm);
    }
    #endregion


    #region mapping
    private MessageModel ToMessageModel(MessageModelView mmv) => _mapService.Map<MessageModelView, MessageModel>(mmv);
    private MessageModelView ToMessageModelView(MessageModel mm) => _mapService.Map<MessageModel, MessageModelView>(mm);
    private List<MessageModel> ToMessageModel(List<MessageModelView> mm) => _mapService.Map<List<MessageModelView>, List<MessageModel>>(mm);
    private List<MessageModelView> ToMessageModelView(List<MessageModel> mm) => _mapService.Map<List<MessageModel>, List<MessageModelView>>(mm);

    private SystemMessageModel ToSystemMessageModel(SystemMessageModelView smmv) => _mapService.Map<SystemMessageModelView, SystemMessageModel>(smmv);
    private SystemMessageModelView ToSystemMessageModelView(SystemMessageModel smm) => _mapService.Map<SystemMessageModel, SystemMessageModelView>(smm);
    private List<SystemMessageModelView> ToSystemMessageModelView(List<SystemMessageModel> mm) => _mapService.Map<List<SystemMessageModel>, List<SystemMessageModelView>>(mm);
    #endregion
}
