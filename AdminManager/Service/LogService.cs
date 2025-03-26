using AdminManager.Accessors;
using AdminManager.Common;
using AdminManager.ModelLog;
using AdminManager.ModelView;
using System.Text;

namespace AdminManager.Service;

public interface ILogService
{
    Task<Tuple<int, List<LogCurrencyDataModelView>>> GetCurrencyLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogTierDataModelView>>> GetTierLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogMailDataModelView>>> GetMailLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogShopDataModelView>>> GetShopLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);
    Task<Tuple<int, List<LogCharDataModelView>>> GetCharacterLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option);


    string ExportToCSV<T>(string name, string header, IEnumerable<T> logTable);
}
public class LogService(IServiceProvider sp) : ILogService
{
    private readonly IGDTAccessor _gdtAccessor = sp.GetRequiredService<IGDTAccessor>();
    private readonly ILogAccessor _logAccessor = sp.GetRequiredService<ILogAccessor>();
    private readonly IMapperService _mapService = sp.GetRequiredService<IMapperService>();

    public async Task<Tuple<int, List<LogCurrencyDataModelView>>> GetCurrencyLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var ret = await _logAccessor.GetCurrencyLogAsync(target, filter, option);
        if (ret == null)
        {
            return null;
        }

        var cnt = ret.Item1;
        var cm = ret.Item2;
        if (cm == null)
        {
            return new Tuple<int, List<LogCurrencyDataModelView>>(cnt, null);
        }

        var clvm = new List<LogCurrencyDataModelView>();
        foreach (var m in cm)
        {
            if (m.Currency == null)
            {
                var vm = new LogCurrencyDataModelView()
                {
                    Type = 0,
                };

                clvm.Add(vm);
                continue;
            }
            
            foreach (var c in m.Currency)
            {
                var vm = ToCurrencyModelView(c);
                vm.FilterStr = Enum.IsDefined(typeof(LogFilterType), m.Filter) ? ((LogFilterType)m.Filter).GetDescription() : m.Filter.ToString();
                vm.ReasonStr = Enum.IsDefined(typeof(LogReasonType), m.Reason) ? ((LogReasonType)m.Reason).GetDescription() : m.Reason.ToString();
                vm.SubTypeStr = _gdtAccessor.GetLangCurrencySubTypeKor(c.Type);
                vm.LogDateTime = m.RegDateTime.ToTimeZone();

                clvm.Add(vm);
            }
        }

        return new Tuple<int, List<LogCurrencyDataModelView>>(cnt, clvm);
    }

    public async Task<Tuple<int, List<LogTierDataModelView>>> GetTierLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var ret = await _logAccessor.GetTierLogAsync(target, filter, option);
        if (ret == null)
        {
            return null;
        }

        var cnt = ret.Item1;
        var cm = ret.Item2;
        if (cm == null)
        {
            return new Tuple<int, List<LogTierDataModelView>>(cnt, null);
        }

        var tiers = new List<LogTierDataModelView>();
        foreach (var m in cm)
        {
            var vm = ToTierModelView(m);
            vm.FilterStr = Enum.IsDefined(typeof(LogFilterType), m.Filter) ? ((LogFilterType)m.Filter).GetDescription() : m.Filter.ToString();
            vm.ReasonStr = Enum.IsDefined(typeof(LogReasonType), m.Reason) ? ((LogReasonType)m.Reason).GetDescription() : m.Reason.ToString();
            vm.LogDateTime = m.RegDateTime.ToTimeZone();

            tiers.Add(vm);
        }

        return new Tuple<int, List<LogTierDataModelView>>(cnt, tiers);
    }

    public async Task<Tuple<int, List<LogMailDataModelView>>> GetMailLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var ret = await _logAccessor.GetMailLogAsync(target, filter, option);
        if (ret == null)
        {
            return null;
        }

        var cnt = ret.Item1;
        var cm = ret.Item2;
        if (cm == null)
        {
            return new Tuple<int, List<LogMailDataModelView>>(cnt, null);
        }

        var mails = new List<LogMailDataModelView>();
        foreach (var m in cm)
        {
            var vm = ToMailViewModel(m);
            vm.FilterStr = Enum.IsDefined(typeof(LogFilterType), m.Filter) ? ((LogFilterType)m.Filter).GetDescription() : m.Filter.ToString();
            vm.ReasonStr = Enum.IsDefined(typeof(LogReasonType), m.Reason) ? ((LogReasonType)m.Reason).GetDescription() : m.Reason.ToString();
            vm.LogDateTime = m.RegDateTime.ToTimeZone();

            mails.Add(vm);
        }

        return new Tuple<int, List<LogMailDataModelView>>(cnt, mails);
    }

    public async Task<Tuple<int, List<LogShopDataModelView>>> GetShopLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var ret = await _logAccessor.GetShopLogAsync(target, filter, option);
        if (ret == null)
        {
            return null;
        }

        var cnt = ret.Item1;
        var cm = ret.Item2;
        if (cm == null)
        {
            return new Tuple<int, List<LogShopDataModelView>>(cnt, null);
        }

        var shops = new List<LogShopDataModelView>();
        foreach (var m in cm)
        {
            var vm = ToShopViewModel(m.Shop);
            vm.IdStr = _gdtAccessor.GetLangProductKor(vm.Id);
            vm.FilterStr = Enum.IsDefined(typeof(LogFilterType), m.Filter) ? ((LogFilterType)m.Filter).GetDescription() : m.Filter.ToString();
            vm.ReasonStr = Enum.IsDefined(typeof(LogReasonType), m.Reason) ? ((LogReasonType) m.Reason).GetDescription() : m.Reason.ToString();
            vm.LogDateTime = m.RegDateTime.ToTimeZone();

            shops.Add(vm);
        }

        return new Tuple<int, List<LogShopDataModelView>>(cnt, shops);
    }

    public async Task<Tuple<int, List<LogCharDataModelView>>> GetCharacterLog(LogSearchTarget target, LogSearchFilter filter, LogSearchOption option)
    {
        var ret = await _logAccessor.GetCharacterLogAsync(target, filter, option);
        if (ret == null)
        {
            return null;
        }

        var cnt = ret.Item1;
        var cm = ret.Item2;
        if (cm == null)
        {
            return new Tuple<int, List<LogCharDataModelView>>(cnt, null);
        }

        var chars = new List<LogCharDataModelView>();
        foreach (var m in cm)
        {
            m.Char ??= new LogCharDataModel()
            {
                CUID = m.User.CharUID
            };

            var vm = ToCharacterViewModel(m);
            vm.FilterStr = Enum.IsDefined(typeof(LogFilterType), m.Filter) ? ((LogFilterType)m.Filter).GetDescription() : m.Filter.ToString();
            vm.ReasonStr = Enum.IsDefined(typeof(LogReasonType), m.Reason) ? ((LogReasonType)m.Reason).GetDescription() : m.Reason.ToString();
            vm.LogDateTime = m.RegDateTime.ToTimeZone();

            chars.Add(vm);
        }

        return new Tuple<int, List<LogCharDataModelView>>(cnt, chars);
    }

    public string ExportToCSV<T>(string name, string header, IEnumerable<T> logTable)
    {
        var csv = new StringBuilder();

        // Add header
        csv.AppendLine(header);

        // Add data
        foreach (var item in logTable)
        {
            var row = string.Join(",", typeof(T).GetProperties().Select(p => p.GetValue(item, null)));
            csv.AppendLine(row);
        }

        var path = Path.Combine(Environment.CurrentDirectory, "download");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Save the CSV file
        var fileName = $"{name}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        var filePath = Path.Combine(path, $"{fileName}.csv");
        File.WriteAllText(filePath, csv.ToString());

        return fileName;
    }

    private LogCurrencyDataModelView ToCurrencyModelView(LogCurrencyDataModel c) => _mapService.Map<LogCurrencyDataModel, LogCurrencyDataModelView>(c);
    private LogTierDataModelView ToTierModelView(LogTierModel m) => _mapService.Map<LogTierDataModel, LogTierDataModelView>(m.Tier);
    private LogMailDataModelView ToMailViewModel(LogMailModel m) => _mapService.Map<LogMailDataModel, LogMailDataModelView>(m.Mail);
    private LogCharDataModelView ToCharacterViewModel(LogCharModel m) => _mapService.Map<LogCharDataModel, LogCharDataModelView>(m.Char);
    private LogShopDataModelView ToShopViewModel(LogShopDataModel m) => _mapService.Map<LogShopDataModel, LogShopDataModelView>(m);
}
