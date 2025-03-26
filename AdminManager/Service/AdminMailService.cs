using AdminManager.Accessors;
using AdminManager.Common;
using AdminManager.Components.Pages;
using AdminManager.Model;
using AdminManager.ModelView;
using Dapper;
using Quartz.Impl.AdoJobStore;
using System;
using System.Data;

namespace AdminManager.Service;

public interface IAdminMailService
{

    Task<bool> AdminMailTargetAccountValidationAsync(List<AdminMailTarget> adminMailTargets);
    Task<bool> AdminMailTargetCharacterValidationAsync(TargetAllType targetType, List<AdminMailTarget> targets);

    Task<bool> AddAdminMailAsync(int mailId, long begin, long end,
        IEnumerable<AdminMailAttachItemModelView> mailItems);

    Task<bool> AddAdminMailToUserAsync(List<AdminMailTarget> targets, int mailId, long begin, long end,
        IEnumerable<AdminMailAttachItemModelView> mailItems);

    Task<bool> AddAdminMailToCharacterAsync(TargetUserType targetUserType, List<AdminMailTarget> targets,
        int mailId, long begin, long end, IEnumerable<AdminMailAttachItemModelView> mailItems);

    IEnumerable<AdminMailAttachItem> GetItemText();
    IEnumerable<MailHeader> GetMailText();
    MailBody GetMailBody(int titleId);

    IEnumerable<AdminCurrencyMainType> GetCurrencyMainTypeText();
    IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText();
    IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText(byte mainType);

    IEnumerable<AdminMailModelView> GetAdminMailEvent();
    IEnumerable<AdminMailModelView> GetAdminAccountMailEvent();
    IEnumerable<AdminMailModelView> GetAdminCharacterMailEvent();

    Task<List<AdminMailAttachItemModelView>> GetAdminMailAttachItemAsync(long amuid);
    Task<List<AdminMailAttachItemModelView>> GetAdminAccountMailAttachItemAsync(long amuid);
    Task<List<AdminMailAttachItemModelView>> GetAdminCharacterMailAttachItemAsync(long amuid);

    Task<bool> RemoveAdminMailAsync(long amuid);
    Task<bool> RemoveAccountAdminMailAsync(long amuid);
    Task<bool> RemoveCharacterAdminMailAsync(long amuid);

    Task SendAdminMailReloadAsync();
    Task SendAccountAdminMailReloadAsync(List<long> auids);
    Task SendCharacterAdminMailReloadAsync(List<long> cuids);
}

public class AdminMailService(IServiceProvider sp) : IAdminMailService
{
    private readonly IDBAccessor _dbAccessor = sp.GetRequiredService<IDBAccessor>();
    private readonly IGDTAccessor _gdtAccessor = sp.GetRequiredService<IGDTAccessor>();
    private readonly IRedisService _redisService = sp.GetRequiredService<IRedisService>();
    private readonly IMapperService _mapService = sp.GetRequiredService<IMapperService>();

    private readonly Dictionary<long, AdminMailModelView> _allMails= new();
    private readonly Dictionary<long, AdminMailModelView> _accountMails = new();
    private readonly Dictionary<long, AdminMailModelView> _characterMails = new();

    public void Load() => Task.Run(InitAsync).Wait();
    public void AllLoad() => Task.Run(InitAllAsync).Wait();
    public void AccountLoad() => Task.Run(InitAccountAsync).Wait();
    public void CharLoad() => Task.Run(InitCharacterAsync).Wait();

    private async Task InitAsync()
    {
        _allMails.Clear();
        _accountMails.Clear();
        _characterMails.Clear();

        foreach (var ms in await GetAdminMailAsync())
        {
            if (!_allMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }

        foreach (var ms in await GetAdminAccountMailAsync())
        {
            if (!_accountMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }

        foreach (var ms in await GetAdminCharacterMailAsync())
        {
            if (!_characterMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }
    }

    private async Task InitAllAsync()
    {
        _allMails.Clear();

        foreach (var ms in await GetAdminMailAsync())
        {
            if (!_allMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }
    }

    private async Task InitAccountAsync()
    {
        _accountMails.Clear();

        foreach (var ms in await GetAdminAccountMailAsync())
        {
            if (!_accountMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }
    }

    private async Task InitCharacterAsync()
    {
        _characterMails.Clear();

        foreach (var ms in await GetAdminCharacterMailAsync())
        {
            if (!_characterMails.TryAdd(ms.MUID, ms))
            {
                continue;
            }
        }
    }

    public IEnumerable<AdminMailModelView> GetAdminMailEvent()
    {
        AllLoad();
        
        var adminMailDesc = _allMails.Values.OrderBy(x => x.MUID).ToList();
        return adminMailDesc;
    }

    public IEnumerable<AdminMailModelView> GetAdminAccountMailEvent()
    {
        AccountLoad();
        
        var adminMailDesc = _accountMails.Values.OrderBy(x => x.MUID).ToList();
        return adminMailDesc;
    }

    public IEnumerable<AdminMailModelView> GetAdminCharacterMailEvent()
    {
        CharLoad();
        
        var adminMailDesc = _characterMails.Values.OrderBy(x => x.MUID).ToList();
        return adminMailDesc;
    }

    public async Task<bool> AdminMailTargetAccountValidationAsync(List<AdminMailTarget> adminMailTargets)
    {
        foreach (var row in adminMailTargets)
        {
            if (row.AccountUId <= 0)
            {
                row.Valid = false;
                return false;
            }

            row.Valid = await _dbAccessor.IsExistsAuid(row.AccountUId);
        }

        return true;
    }

    public async Task<bool> AdminMailTargetCharacterValidationAsync(TargetAllType targetType, List<AdminMailTarget> targets)
    {
        switch (targetType)
        {
            case TargetAllType.CharacterName:
                foreach (var row in targets)
                {
                    if (string.IsNullOrEmpty(row.CharName))
                    {
                        row.Valid = false;
                        return false;
                    }

                    var character = await _dbAccessor.GetCharacterAsync(row.CharName);
                    if (character == null)
                    {
                        row.Valid = false;
                        return false;
                    }

                    row.CharUId = character.CUID;
                    row.AccountUId = character.AUID;
                    row.Valid = true;
                }
                break;
            case TargetAllType.CharacterUId:
                foreach (var row in targets)
                {
                    if (row.CharUId <= 0)
                    {
                        row.Valid = false;
                        return false;
                    }

                    var character = await _dbAccessor.GetCharacterAsync(row.CharUId);
                    if (character == null)
                    {
                        row.Valid = false;
                        return false;
                    }

                    row.AccountUId = character.AUID;
                    row.CharName = character.Name;
                    row.Valid = true;
                }
                break;
            default:
                return false;
        }

        return true;
    }

    public async Task<bool> AddAdminMailAsync(int mailId, long begin, long end, IEnumerable<AdminMailAttachItemModelView> mailItems)
    {
        var items = ToMailAttachItemModel(mailItems);
        return await _dbAccessor.AddAdminMailAsync(mailId, begin, end, items);
    }

    private List<AdminMailAttachItemModel> ToMailAttachItemModel(IEnumerable<AdminMailAttachItemModelView> mailItems)
    {
        List<AdminMailAttachItemModel> mailAttachItemModel = new();

        foreach (var item in mailItems)
        {
            mailAttachItemModel.Add(
                new AdminMailAttachItemModel()
                {
                    ItemType = Convert.ToByte(item.MainType),
                    Param = (item.MainType == GDT.CurrencyMainType.CMT_Item)
                        ? item.ItemId
                        : Convert.ToByte(item.SubType),
                    Amount = item.Amount
                }
            );
        }

        return mailAttachItemModel;
    }

    public async Task<bool> AddAdminMailToUserAsync(List<AdminMailTarget> targets, int mailId, long begin, long end, IEnumerable<AdminMailAttachItemModelView> mailItems)
    {
        var mailAttachItems = ToMailAttachItemModel(mailItems);

        foreach (var target in targets)
        {
            if (!await _dbAccessor.AddAdminMailToUserAsync(target.AccountUId, mailId, begin, end, mailAttachItems))
            {
                return false;
            }

            target.Send = true;
        }

        return true;
    }

    public async Task<bool> AddAdminMailToCharacterAsync(TargetUserType targetUserType, List<AdminMailTarget> targets, int mailId, long begin, long end, IEnumerable<AdminMailAttachItemModelView> mailItems)
    {
        var mailAttachItems = ToMailAttachItemModel(mailItems);
        foreach (var target in targets)
        {
            switch (targetUserType)
            {
                case TargetUserType.Account:
                    if (!await _dbAccessor.AddAdminMailToUserAsync(target.AccountUId, mailId, begin, end, mailAttachItems))
                    {
                        return false;
                    }
                    break;
                case TargetUserType.Character:
                    if (!await _dbAccessor.AddAdminMailToCharacterAsync(target.CharUId, mailId, begin, end, mailAttachItems))
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            target.Send = true;
        }

        return true;
    }

    public IEnumerable<AdminMailAttachItem> GetItemText() => _gdtAccessor.GetItemText();
    public IEnumerable<MailHeader> GetMailText() => _gdtAccessor.GetMailText();
    public MailBody GetMailBody(int titleId) => _gdtAccessor.GetPresetByMailId(titleId);

    public IEnumerable<AdminCurrencyMainType> GetCurrencyMainTypeText() => _gdtAccessor.GetCurrencyMainTypeText();
    public IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText() => _gdtAccessor.GetCurrencySubTypeText();
    public IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText(byte mainType) => _gdtAccessor.GetCurrencySubTypeText(mainType);

    private async Task<List<AdminMailModelView>> GetAdminMailAsync()
    {
        var models = await _dbAccessor.GetAdminMailAsync();
        return ToAdminMailModelView(models); 
    }

    private async Task<List<AdminMailModelView>> GetAdminCharacterMailAsync()
    {
        var models = await _dbAccessor.GetAdminCharacterMailAsync();
        return ToAdminMailModelView(models);
    }
    private async Task<List<AdminMailModelView>> GetAdminAccountMailAsync()
    {
        var models = await _dbAccessor.GetAdminAccountMailAsync();
        return ToAdminMailModelView(models);
    }

    public async Task<List<AdminMailAttachItemModelView>> GetAdminMailAttachItemAsync(long amuid)
    {
        var modelItems = await _dbAccessor.GetAdminMailAttachItem(amuid);
        return ToAdminMailAttachItemModelView(modelItems);
    }

    public async Task<List<AdminMailAttachItemModelView>> GetAdminCharacterMailAttachItemAsync(long amuid)
    {
        var modelItems = await _dbAccessor.GetAdminCharacterMailAttachItemAsync(amuid);
        return ToAdminMailAttachItemModelView(modelItems);
    }

    public async Task<List<AdminMailAttachItemModelView>> GetAdminAccountMailAttachItemAsync(long amuid)
    {
        var modelItems = await _dbAccessor.GetAdminAccountMailAttachItemAsync(amuid);
        return ToAdminMailAttachItemModelView(modelItems);
    }


    public async Task<bool> RemoveAdminMailAsync(long amuid)
    {
        if (amuid == 0)
        {
            return false;
        }

        if (!_allMails.ContainsKey(amuid))
        {
            return false;
        }

        if (!await _dbAccessor.RemoveAdminMailAsync(amuid))
        {
            return false;
        }

        _allMails.Remove(amuid);

        await SendAdminMailReloadAsync();

        return true;
    }
    public async Task<bool> RemoveAccountAdminMailAsync(long amuid)
    {
        if (amuid == 0)
        {
            return false;
        }

        if (!_accountMails.ContainsKey(amuid))
        {
            return false;
        }

        if (!await _dbAccessor.RemoveAccountAdminMailAsync(amuid))
        {
            return false;
        }

        var auids = _accountMails.Values.Select(i => i.AUID).ToList();
        await SendAccountAdminMailReloadAsync(auids);

        _accountMails.Remove(amuid);

        return true;
    }

    public async Task<bool> RemoveCharacterAdminMailAsync(long amuid)
    {
        if (amuid == 0)
        {
            return false;
        }

        if (!_characterMails.ContainsKey(amuid))
        {
            return false;
        }

        if (!await _dbAccessor.RemoveCharacterAdminMailAsync(amuid))
        {
            return false;
        }

        var cuids = _characterMails.Values.Select(i => i.CUID).ToList();
        await SendCharacterAdminMailReloadAsync(cuids);

        _characterMails.Remove(amuid);

        return true;
    }



    public async Task SendAdminMailReloadAsync()
    {
        await _redisService.SendAdminMailReloadAsync();
    }

    public async Task SendAccountAdminMailReloadAsync(List<long> auids)
    {
        if (auids.Count == 0)
            return;

        await _redisService.SendAccountAdminMailReloadCommandAsync(auids);
    }

    public async Task SendCharacterAdminMailReloadAsync(List<long> cuids)
    {
        if (cuids.Count == 0)
            return;

        await _redisService.SendCharacterAdminMailReloadCommandAsync(cuids);
    }


    private List<AdminMailModelView> ToAdminMailModelView(List<AdminMailModel> amm) => _mapService.Map<List<AdminMailModel>, List<AdminMailModelView>>(amm);
    private List<AdminMailAttachItemModelView> ToAdminMailAttachItemModelView(List<AdminMailAttachItemModel> amm) => _mapService.Map<List<AdminMailAttachItemModel>, List<AdminMailAttachItemModelView>>(amm);
}

