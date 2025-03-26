using AdminManager.Common;
using AdminManager.ModelLog;
using AdminManager.ModelLog.Shared;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AdminManager.Accessors;
public interface ILogAccessor
{
    Task<Tuple<int, List<LogCurrencyModel>>> GetCurrencyLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogTierModel>>> GetTierLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogMailModel>>> GetMailLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogShopModel>>> GetShopLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogCharModel>>> GetCharacterLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
}

public class LogAccessor : ILogAccessor
{
    private readonly MongoProvider _mongo;

    public LogAccessor(MongoProvider mp)
    {
        _mongo = mp;

        Init();
    }

    private void Init()
    {
        LogModelMapper.Init();
        LogCurrencyMapper.Init();
        LogTierMapper.Init();
        LogMailMapper.Init();
        LogShopMapper.Init();
        LogCharMapper.Init();
    }

    public async Task<Tuple<int, List<LogCurrencyModel>>> GetCurrencyLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var query = GetLogTargetQuery<LogCurrencyModel>(target, "currency");
        if (query == null)
        {
            return new Tuple<int, List<LogCurrencyModel>>(0, null);
        }

        query = GetLogFilterQuery(query, filter);
        if (query == null)
        {
            return new Tuple<int, List<LogCurrencyModel>>(0, null);
        }

        var subType = Convert.ToByte(filter.SubType);
        if (subType > 0)
        {
            query = query.Where(c => c.Currency.Any(lc => lc.Type == subType));
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new Tuple<int, List<LogCurrencyModel>>(0, null);
        }

        query = option.IsAsc ? query.OrderBy(c => c.RegDateTime) : query.OrderByDescending(c => c.RegDateTime);
        var ret = await query.Skip(option.Size).Take(option.Take).ToListAsync();

        return new Tuple<int, List<LogCurrencyModel>>(count, ret);
    }

    public async Task<Tuple<int, List<LogTierModel>>> GetTierLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var query = GetLogTargetQuery<LogTierModel>(target, "tier");
        if (query == null)
        {
            return new Tuple<int, List<LogTierModel>>(0, null);
        }

        query = GetLogFilterQuery(query, filter);
        if (query == null)
        {
            return new Tuple<int, List<LogTierModel>>(0, null);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new Tuple<int, List<LogTierModel>>(0, null);
        }

        if (option.IsAsc)
        {
            query = query.OrderBy(c => c.RegDateTime);
        }
        else
        {
            query = query.OrderByDescending(c => c.RegDateTime);
        }
        var ret = await query.Skip(option.Size).Take(option.Take).ToListAsync();

        return new Tuple<int, List<LogTierModel>>(count, ret);
    }

    public async Task<Tuple<int, List<LogMailModel>>> GetMailLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var query = GetLogTargetQuery<LogMailModel>(target, "mail");
        if (query == null)
        {
            return new Tuple<int, List<LogMailModel>>(0, null);
        }

        query = GetLogFilterQuery(query, filter);
        if (query == null)
        {
            return new Tuple<int, List<LogMailModel>>(0, null);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new Tuple<int, List<LogMailModel>>(0, null);
        }

        query = option.IsAsc ? query.OrderBy(c => c.RegDateTime) : query.OrderByDescending(c => c.RegDateTime);
        var ret = await query.Skip(option.Size).Take(option.Take).ToListAsync();

        return new Tuple<int, List<LogMailModel>>(count, ret);
    }

    public async Task<Tuple<int, List<LogShopModel>>> GetShopLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var query = GetLogTargetQuery<LogShopModel>(target, "shop");
        if (query == null)
        {
            return new Tuple<int, List<LogShopModel>>(0, null);
        }

        query = GetLogFilterQuery(query, filter);
        if (query == null)
        {
            return new Tuple<int, List<LogShopModel>>(0, null);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new Tuple<int, List<LogShopModel>>(0, null);
        }

        query = option.IsAsc ? query.OrderBy(c => c.RegDateTime) : query.OrderByDescending(c => c.RegDateTime);
        var ret = await query.Skip(option.Size).Take(option.Take).ToListAsync();

        return new Tuple<int, List<LogShopModel>>(count, ret);
    }

    public async Task<Tuple<int, List<LogCharModel>>> GetCharacterLogAsync(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var query = GetLogTargetQuery<LogCharModel>(target, "character");
        if (query == null)
        {
            return new Tuple<int, List<LogCharModel>>(0, null);
        }

        query = GetLogFilterQuery(query, filter);
        if (query == null)
        {
            return new Tuple<int, List<LogCharModel>>(0, null);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new Tuple<int, List<LogCharModel>>(0, null);
        }

        query = option.IsAsc ? query.OrderBy(c => c.RegDateTime) : query.OrderByDescending(c => c.RegDateTime);
        var ret = await query.Skip(option.Size).Take(option.Take).ToListAsync();

        return new Tuple<int, List<LogCharModel>>(count, ret);
    }

    private IMongoQueryable<T> GetLogTargetQuery<T>(LogSearchTarget target, string name) where T : LogModelBase
    {
        var cuid = target.CUID;
        var begin = target.BeginDateTime;
        var end = target.EndDateTime;

        var db = _mongo.Open();
        var collection = db.GetCollection<T>(name).AsQueryable();
        var targetQuery = collection.Where(c => c.User.CharUID == cuid && c.RegDateTime >= begin && c.RegDateTime <= end);

        return targetQuery;
    }

    private IMongoQueryable<T> GetLogFilterQuery<T>(IMongoQueryable<T> query, LogSearchFilter filter) where T : LogModelBase
    {
        var filt = Convert.ToInt32(filter.FilterType);
        var reason = Convert.ToInt32(filter.ReasonType);

        if (filt > 0)
        {
            query = query.Where(c => c.Filter == filt);
        }

        if (reason > 0)
        {
            query = query.Where(c => c.Reason == reason);
        }

        return query;
    }
}
