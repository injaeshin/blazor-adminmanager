using GDT;
using AdminManager.Common;
using AdminManager.ModelView;

namespace AdminManager.Accessors;

public interface IGDTAccessor
{
    public string GetLangCurrencyMainTypeToString(int key);
    public string GetLangCurrencySubTypeKor(int key);
    public string ToLanguageKorByItemId(int key);
    //public string GetLangCoinKor(int key);
    public string ToLanguageKorByType(byte type, int param);

    public IEnumerable<MailHeader> GetMailText();
    public MailBody GetPresetByMailId(int mailId);

    //public bool IsValidAmount(byte itemType, int param, int amount);
    public string GetVersion();
    IEnumerable<AdminCurrencyMainType> GetCurrencyMainTypeText();
    IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText();
    IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText(byte mainType);
    IEnumerable<AdminMailAttachItem> GetItemText();

    //IEnumerable<EventType> GetEventText();

    public string GetQuestName(int key);
    public string GetQuestTypeText(int key);

    public int GetQuestTaskByQuestId(int key);
    public string GetQuestTaskName(int key);
    public string GetQuestTaskTypeText(int key);
    public string GetLangProductKor(int vmId);
}

public class GDTAccessor : IGDTAccessor
{
    private const string None = "none";
    private CommonGDTManager _gdt => GetGDT();
    private CommonGDTManager _gdtManager = null;

    private readonly GDTProvider _gdtProvider;
    private Dictionary<CurrencyMainType, List<CurrencySubType>> _currencySubType;

    public GDTAccessor(GDTProvider gdtProvider)
    {
        _gdtProvider = gdtProvider;
        _currencySubType = new();

        Init();
        InitCurrencySubType();
    }

    public List<CurrencySubType> GetCurrencySubType(CurrencyMainType type)
    {
        return _currencySubType[type];
    }

    private void InitCurrencySubType()
    {
        _currencySubType ??= new();
        _currencySubType.Clear();

        foreach (var type in Enum.GetValues(typeof(CurrencyMainType)))
        {
            var i = _gdt.CurrencyInfoTable.Values
                .Where(i => i.CurrencyMainTypeID == (CurrencyMainType)type)
                .Select(i => i.CurrencySubTypeID)
                .ToList();

            if (i.Count > 0)
                _currencySubType[(CurrencyMainType)type] = i;
        }

        foreach (var type in Enum.GetValues(typeof(CurrencyMainType)))
        {
            if (_currencySubType.ContainsKey((CurrencyMainType)type))
                continue;

            var i = _gdt.CoinInfoTable.Values
                .Where(i => i.CurrencyMainTypeID == (CurrencyMainType)type)
                .Select(i => i.CurrencySubTypeID)
                .ToList();

            if (i.Count > 0)
                _currencySubType[(CurrencyMainType)type] = i;
        }
    }

    private void Init() => _gdtManager = _gdtProvider.Open();

    public void Clear() => _gdtManager = null;

    private CommonGDTManager GetGDT()
    {
        if (_gdtManager == null)
        {
            Init();
        }

        return _gdtManager;
    }

    private static string GetEnumName<T>(int key)
    {
        var name = Enum.GetName(typeof(T), key);
        return name ?? None;
    }

    public string GetVersion()
    {
        return $"{_gdt?.MajorVersion}.{_gdt?.MinorVersion}";
    }

    public IEnumerable<AdminCurrencyMainType> GetCurrencyMainTypeText()
    {
        foreach (var obj in Enum.GetValues(typeof(CurrencyMainType)))
        {
            var key = (byte)obj;
            yield return new AdminCurrencyMainType()
            {
                MainType = (CurrencyMainType)key,
                MainTypeStr = GetEnumName<CurrencyMainType>(key)
            };
        }
    }
    public IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText()
    {
        foreach (var obj in Enum.GetValues(typeof(CurrencySubType)))
        {
            var key = (byte)obj;
            yield return new AdminCurrencySubType()
            {
                SubType = (CurrencySubType)key,
                SubTypeStr = GetEnumName<CurrencySubType>(key)
            };
        }
    }

    public IEnumerable<AdminCurrencySubType> GetCurrencySubTypeText(byte mainType)
    {
        if (!_currencySubType.ContainsKey((CurrencyMainType)mainType))
            return Enumerable.Empty<AdminCurrencySubType>();

        return from key in _currencySubType[(CurrencyMainType)mainType]
               select new AdminCurrencySubType()
               {
                   SubType = key,
                   SubTypeStr = GetEnumName<CurrencySubType>((int)key)
               };

        //return from byte key in Enum.GetValues(typeof(CurrencySubType))
        //       select new AdminCurrencySubType()
        //       {
        //           SubType = (CurrencySubType)key,
        //           SubTypeStr = GetEnumName<CurrencySubType>(key)
        //       };
    }

    public IEnumerable<AdminMailAttachItem> GetItemText()
    {
        return _gdt?.ItemInfoTable.Values!.Select(item => new AdminMailAttachItem()
        {
            ItemId = item.ItemID,
            ItemStr = $"[{item.ItemID}] [{_gdt.GetLanguageItem(item.ItemName)?.DescriptionKor}]", // $"{item.ItemID} {item.ItemName}",
            MaxAmount = item.ItemStackMax
        });
    }

    //public IEnumerable<EventType> GetEventText()
    //{
    //    return _gdt?.EventInfoTable.Values.Select(x => new EventType
    //    { 
    //        EventStr = $"[{x.EventID}] [{_gdt.GetLanguageUI(x.EventName)?.DescriptionKor}]",
    //        EventId = x.EventID
    //    });
    //}

    public string GetLangCurrencyMainTypeToString(int key)
    {
        return GetEnumName<CurrencyMainType>(key).ToString();
    }

    public string GetLangCurrencySubTypeKor(int key)
    {
        var ccy = _gdt?.GetCurrencyInfo((GDT.CurrencySubType)key);
        if (ccy is null)
            return GetEnumName<GDT.CurrencySubType>(key);

        var lang = _gdt?.GetLanguageUI(ccy.CurrencyName);
        if (lang is null)
            return GetEnumName<GDT.CurrencySubType>(key);

        return lang.DescriptionKor;
    }

    public string ToLanguageKorByItemId(int key)
    {
        var item = _gdt.GetItemInfo(key);
        if (item is null)
            return None;

        var lang = _gdt.GetLanguageItem(item.ItemName);
        if (lang is null)
            return item.ItemName;

        return lang.DescriptionKor;
    }

    private string GetLangCoinKor(int key)
    {
        var coin = _gdt.GetCoinInfo((GDT.CurrencySubType)key);
        if (coin is null)
            return None;

        var lang = _gdt?.GetLanguageUI(coin.CurrencyName);
        if (lang is null)
            return GetEnumName<GDT.CurrencySubType>(key);

        return lang.DescriptionKor;
    }

    public string ToLanguageKorByType(byte type, int param)
    {
        try
        {
            return (GDT.CurrencyMainType)type switch
            {
                GDT.CurrencyMainType.CMT_Gold or GDT.CurrencyMainType.CMT_Ticket => GetLangCurrencySubTypeKor(param),
                GDT.CurrencyMainType.CMT_Coin => GetLangCoinKor(param),
                GDT.CurrencyMainType.CMT_Item => ToLanguageKorByItemId(param),
                _ => None,
            };
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex);
            return None;
        }
    }

    public IEnumerable<MailHeader> GetMailText()
    {
        return _gdt?.MailTable.Select(x =>
            new MailHeader
            {
                Id = x.Value.Index,
                Title = x.Value.MailTitle
            }).ToList();
    }

    public MailBody GetPresetByMailId(int mailId)
    {
        var detail = new MailBody();

        var mail = _gdt?.GetMail(mailId);
        if (mail is null)
            return default;

        var langTitle = _gdt?.GetLanguageUI(mail.MailTitle);
        if (langTitle is null)
            return default;

        var langText = _gdt?.GetLanguageUI(mail.MailText);
        if (langText is null)
            return default;

        var langFrom = _gdt?.GetLanguageUI(mail.MailFrom);
        if (langFrom is null)
            return default;

        detail.Title = langTitle.DescriptionKor;
        detail.Body = langText.DescriptionKor;
        detail.Name = langFrom.DescriptionKor;
        detail.Term = mail.MailTerm;
        detail.KeepType = mail.KeepTypeID.ToString();
        detail.AccountType = mail.MailAccountTypeID;

        return detail;
    }


    //public bool IsValidAmount(byte itemType, int param, int amount)
    //{
    //    if (itemType != 3)
    //        return true;

    //    var item = _gdt?.GetItemInfo(param);
    //    if (item is null)
    //        return false;

    //    return item.ItemStackMax >= amount;
    //}

    public string GetQuestName(int key)
    {
        var quest = _gdt?.GetQuestInfo(key);
        if(quest is null)
            return None;

        var lang = _gdt?.GetLanguageQuest(quest.QuestTitle);
        if(lang is null) 
            return None;

        return lang.DescriptionKor;
    }
    public string GetQuestTypeText(int key)
    {
        var quest = _gdt?.GetQuestInfo(key);
        if (quest is null)
            return None;

        var name = Enum.GetName(quest.QuestType);
        if (name is null)
            return None;

        return name;
    }

    public int GetQuestTaskByQuestId(int key)
    {
        var questTask = _gdt?.GetQuestComplete(key);
        if (questTask is null)
            return 0;

        return questTask.QuestID;
    }

    public string GetQuestTaskName(int key)
    {
        var questTask = _gdt?.GetQuestComplete(key);
        if (questTask is null)
            return None;

        var lang = _gdt?.GetLanguageQuest(questTask.QuestHudText);
        if (lang is null)
            return None;

        return lang.DescriptionKor;
    }

    public string GetQuestTaskTypeText(int key)
    {
        var questTask = _gdt?.GetQuestComplete(key);
        if (questTask is null)
            return None;

        var name = Enum.GetName(questTask.QuestConditionType);
        if(name is null)
            return None;

        return name;
    }

    public string GetLangProductKor(int id)
    {
        var prodcut = _gdt?.GetCashShopProduct(id);
        if (prodcut is null)
            return None;

        var lang = _gdt?.GetLanguageShop(prodcut.TitleText);
        if (lang is null)
            return None;

        return lang.DescriptionKor;
    }
}
