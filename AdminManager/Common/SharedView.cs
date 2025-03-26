using GDT;

namespace AdminManager.Common;

public class User
{
    public TargetUserType Type { get; set; }
    public string TypeStr { get; set; }
}

public class Character
{
    public TargetCharacterType Type { get; set; }
    public string TypeStr { get; set; }
}

public class MailTarget
{
    public TargetAllType Type { get; set; }
    public string TypeStr { get; set; }
}

public static class TargetViewPreset
{
    public static readonly List<User> UserTargets =
    [
        new User { Type = TargetUserType.Account, TypeStr = "계정" },
        new User { Type = TargetUserType.Character, TypeStr = "캐릭터" }
    ];

    public static readonly List<Character> CharacterTargets =
    [
        new Character { Type = TargetCharacterType.CharacterUID, TypeStr = "캐릭터 UID" },
        new Character { Type = TargetCharacterType.CharacterName, TypeStr = "캐릭터 이름" }
    ];

    public static readonly List<MailTarget> MailTargets =
    [
        new MailTarget { Type = TargetAllType.AccountUId, TypeStr = "계정_UID" },
        new MailTarget { Type = TargetAllType.CharacterUId, TypeStr = "캐릭터_UID" },
        new MailTarget { Type = TargetAllType.CharacterName, TypeStr = "캐릭터_이름" }
    ];
}

#region SystemMessage (Notify)

public class MessageAction
{
    public MessageType Type { get; set; }
    public string TypeStr { get; set; }
}

public class MessageInterval
{
    public MessageIntervalType Type { get; set; }
    public string TypeStr { get; set; }
}

public class MessageLanguage
{
    public GDT.LanguageType Type { get; set; }
    public string TypeStr { get; set; }
}

public static class MessageViewPreset
{
    public static readonly List<MessageAction> MessageActions =
    [
        new MessageAction { Type = MessageType.Instantly, TypeStr = "즉시" },
        new MessageAction { Type = MessageType.Schedule, TypeStr = "예약" }
    ];

    public static readonly List<MessageInterval> MessageIntervals =
    [
        new MessageInterval { Type = MessageIntervalType.OneMinute, TypeStr = "1분" },
        new MessageInterval { Type = MessageIntervalType.FiveMinute, TypeStr = "5분" },
        new MessageInterval { Type = MessageIntervalType.TenMinute, TypeStr = "10분" },
        new MessageInterval { Type = MessageIntervalType.TwentyFiveMinute, TypeStr = "25분" },
        new MessageInterval { Type = MessageIntervalType.ThirtyMinute, TypeStr = "30분" }
    ];
}

#endregion

#region Log
public class LogFilter
{
    public LogFilterType Type { get; set; }
    public string TypeStr { get; set; }
}

public class LogReason
{
    public LogReasonType Type { get; set; }
    public string TypeStr { get; set; }
}

public static class LogCategory
{
    public static List<LogFilter> GetFilter(LogCateType cate)
    {
        var filters = new List<LogFilter>();
        foreach (LogFilterType filter in Enum.GetValues(typeof(LogFilterType)))
        {
            if ((int)filter / 100 * 100 == (int)cate)
            {
                filters.Add(new LogFilter { Type = filter, TypeStr = filter.GetDescription() });
            }
        }

        filters.First().Type = 0;
        filters.First().TypeStr = "None";

        return filters;
    }
    

    //public static readonly List<LogFilter> Filters =
    //[
    //    new LogFilter { Type = LogFilterType.None, TypeStr = "전체" },
    //    new LogFilter { Type = LogFilterType.CurrencyIncrease, TypeStr = "재화 증가" },
    //    new LogFilter { Type = LogFilterType.CurrencyIncreaseFailure, TypeStr = "재화 증가 실패" },
    //    new LogFilter { Type = LogFilterType.CurrencyDecrease, TypeStr = "재화 감소" },
    //    new LogFilter { Type = LogFilterType.CurrencyDecreaseFailure, TypeStr = "재화 감소 실패" },
    //];

    public static List<LogReason> GetReasons()
    {
        var reasons = new List<LogReason>();
        foreach (LogReasonType reason in Enum.GetValues(typeof(LogReasonType)))
        {
            reasons.Add(new LogReason { Type = reason, TypeStr = reason.GetDescription() });
        }

        return reasons;
    }
}

public class LogSearchEventArgs
{
    public LogSearchTarget Target { get; set; }
    public LogSearchFilter Filter { get; set; }
    public LogSearchOption Option { get; set; }
}

public class LogSearchTarget
{
    public TargetCharacterType CharType { get; set; }
    public long CUID { get; set; }

    public DateTime BeginDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}

public class LogSearchFilter
{
    public LogFilterType FilterType { get; set; }
    public GDT.CurrencySubType SubType { get; set; }
    public LogReasonType ReasonType { get; set; }
}

public class LogSearchOption
{
    public LogSearchOption(int size, int task, bool isAsc = false)
    {
        Size = size;
        Take = task;
        IsAsc = isAsc;
    }

    public int Size { get; set; }
    public int Take { get; set; }
    public bool IsAsc { get; set; }
}
#endregion

public class AdminCurrencyMainType
{
    public GDT.CurrencyMainType MainType { get; set; }
    public string MainTypeStr { get; set; }
}

public class AdminCurrencySubType
{
    public GDT.CurrencySubType SubType { get; set; }
    public string SubTypeStr { get; set; }
}

