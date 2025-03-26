using AdminManager.Accessors;
using AdminManager.Common;
using AdminManager.Model;
using AdminManager.ModelView;
using Nelibur.ObjectMapper;

namespace AdminManager.Service;

public interface IUserService
{
    /// Account
    Task<long> GetAccountUIDAsync(TargetUserType targetUserType, string name);
    Task<AccountModelView> GetAccountDetail(long auid);
    Task<AccountTierModelView> GetAccountTier(long auid);

    /// Character
    Task<long> GetCharacterUID(TargetCharacterType charType, string name);
    Task<IEnumerable<CharacterModelView>> GetCharactersByAUIDAsync(long auid);
    Task<CharacterDetailModelView> GetCharacterDetailByCUID(long cuid);
    
    /// Currency
    Task<IEnumerable<CurrencyModelView>> GetCurrencyAsync(long cuid);

    /// Item
    Task<IEnumerable<ItemModelView>> GetItemAsync(long cuid);

    /// Mail
    Task<IEnumerable<MailModelView>> GetMailAsync(long auid, long cuid);
    Task<IEnumerable<MailAttachItemModelView>> GetMailAttachItem(bool isAccount, long muid);

    /// Quest
    Task<IEnumerable<QuestModelView>> GetQuestAsync(long cuid);
    Task<IEnumerable<QuestTaskModelView>> GetQuestTask(long cuid, int questId);
}

public class UserService(IServiceProvider sp) : IUserService
{
    private readonly IDBAccessor _dbAccessor = sp.GetRequiredService<IDBAccessor>();
    private readonly IMapperService _mapService = sp.GetRequiredService<IMapperService>();
    private readonly IGDTAccessor _gdtAccessor = sp.GetRequiredService<IGDTAccessor>();
    private readonly ILogAccessor _logAccessor = sp.GetRequiredService<ILogAccessor>();
    private readonly IRedisService _redisService = sp.GetRequiredService<IRedisService>();

    #region Account

    public async Task<long> GetAccountUIDAsync(TargetUserType targetUserType, string name)
    {
        return targetUserType switch
        {
            TargetUserType.Account => await _dbAccessor.GetAuidByAccountNameAsync(name),
            TargetUserType.Character => await _dbAccessor.GetAuidByCharacterNameAsync(name),
            _ => 0
        };
    }

    public async Task<AccountModelView> GetAccountDetail(long auid)
    {
        var m = await _dbAccessor.GetAccountAsync(auid);
        return _mapService.Map<AccountModel, AccountModelView>(m);
    }

    public async Task<AccountTierModelView> GetAccountTier(long auid)
    {
        var m = await _dbAccessor.GetTierAsync(auid);
        return _mapService.Map<AccountTierModel, AccountTierModelView>(m);
    }
    #endregion

    #region Character
    public async Task<long> GetCharacterUID(TargetCharacterType charType, string name)
    {
        if (charType == TargetCharacterType.CharacterName)
        {
            var ret = await _dbAccessor.GetCharacterAsync(name);
            return ret?.CUID ?? 0;
        }

        if (charType == TargetCharacterType.CharacterUID)
        {
            return Convert.ToInt64(name);
        }

        return 0;
    }

    public async Task<IEnumerable<CharacterModelView>> GetCharactersByAUIDAsync(long auid)
    {
        var cms = await _dbAccessor.GetCharactersAsync(auid);
        if (cms == null)
        {
            return null;
        }

        var cvms = new List<CharacterModelView>();
        foreach (var cm in cms)
        {
            cvms.Add(_mapService.Map<CharacterModel, CharacterModelView>(cm));
        }

        return cvms;
    }

    public async Task<CharacterDetailModelView> GetCharacterDetailByCUID(long cuid)
    {
        var m = await _dbAccessor.GetCharacterAsync(cuid);
        if (m == null)
        {
            return null;
        }

        return _mapService.Map<CharacterDetailModel, CharacterDetailModelView>(m);
    }
    #endregion

    #region Currency
    public async Task<IEnumerable<CurrencyModelView>> GetCurrencyAsync(long cuid)
    {
        var ms = await _dbAccessor.GetCurrencyAsync(cuid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<CurrencyModelView>();

        foreach (var m in ms)
        {
            var vm = _mapService.Map<CurrencyModel, CurrencyModelView>(m);
            vm.SubTypeStr = _gdtAccessor.GetLangCurrencySubTypeKor(m.SubType);

            vms.Add(vm);
        }

        return vms;
    }
    #endregion

    #region Item
    public async Task<IEnumerable<ItemModelView>> GetItemAsync(long cuid)
    {
        var ms = await _dbAccessor.GetItemAsync(cuid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<ItemModelView>();

        var tp = Convert.ToByte(GDT.CurrencyMainType.CMT_Item);
        foreach (var m in ms)
        {
            var vm = _mapService.Map<ItemModel, ItemModelView>(m);
            vm.ItemName = _gdtAccessor.ToLanguageKorByType(tp, m.ItemID);

            vms.Add(vm);
        }

        return vms;
    }
    #endregion

    #region Mail
    public async Task<IEnumerable<MailModelView>> GetMailAsync(long auid, long cuid)
    {
        var ms = await _dbAccessor.GetMailAsync(auid, cuid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<MailModelView>();
        foreach (var m in ms)
        {
            vms.Add(_mapService.Map<MailModel, MailModelView>(m));
        }

        return vms;
    }

    public async Task<IEnumerable<MailAttachItemModelView>> GetMailAttachItem(bool isAccount, long muid)
    {
        var ms = await _dbAccessor.GetMailItemByMuid(isAccount, muid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<MailAttachItemModelView>();
        foreach (var m in ms)
        {
            var vm = _mapService.Map<MailAttachItemModel, MailAttachItemModelView>(m);
            vm.ItemTypeStr = _gdtAccessor.GetLangCurrencyMainTypeToString(m.ItemType);
            vm.ItemName = _gdtAccessor.ToLanguageKorByType(m.ItemType, m.Param);

            vms.Add(vm);
        }

        return vms;
    }
    #endregion

    #region Quest
    public async Task<IEnumerable<QuestModelView>> GetQuestAsync(long cuid)
    {
        var ms = await _dbAccessor.GetQuestAsync(cuid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<QuestModelView>();
        foreach (var m in ms)
        {
            var cvm = _mapService.Map<QuestModel, QuestModelView>(m);
            cvm.TypeStr = _gdtAccessor.GetQuestTypeText(m.QuestID);
            cvm.Name = _gdtAccessor.GetQuestName(m.QuestID);

            vms.Add(cvm);
        };

        return vms;
    }

    public async Task<IEnumerable<QuestTaskModelView>> GetQuestTask(long cuid, int questId)
    {
        var ms = await _dbAccessor.GetQuestTaskAsync(cuid);
        if (ms == null)
        {
            return null;
        }

        var vms = new List<QuestTaskModelView>();

        foreach (var m in ms)
        {
            if (questId != _gdtAccessor.GetQuestTaskByQuestId(m.TaskID))
            {
                continue;
            }

            var cvm = _mapService.Map<QuestTaskModel, QuestTaskModelView>(m);
            cvm.TypeStr = _gdtAccessor.GetQuestTaskTypeText(m.TaskID);
            cvm.Name = _gdtAccessor.GetQuestTaskName(m.TaskID);

            vms.Add(cvm);
        }

        return vms;
    }
    #endregion
}



