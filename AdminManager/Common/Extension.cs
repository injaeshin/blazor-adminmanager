using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdminManager.Common;

public static class Extension
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }

    public static int ToEventId(this GameType value)
    {
        return value switch
        {
            GameType.None => 0,
            GameType.FutureHunter => 1001,
            GameType.Holdem => 2013,
            GameType.Event => 9001,
            _ => 0
        };
    }

    public static CharStatus GetCharacterStatus(this DateTime value)
    {
        if (value == Utility.EmptyDateTime)
            return CharStatus.Active;
        
        if (DateTime.Compare(value, DateTime.Now) < 0)
            return CharStatus.DeleteSoon;
        
        return CharStatus.Deleted;
    }

    public static DateTime ToTimeZone(this DateTime dt)
    {
        return DateTime.SpecifyKind(dt.AddHours(Utility.TimeOffset), DateTimeKind.Local);
    }

    public static string ToAWFormat(this DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool IsInRange(this DateTime dateTime, long beginTimestamp, long endTimestamp)
    {
        long ts = ((DateTimeOffset)dateTime.ToUniversalTime()).ToUnixTimeSeconds();
        return ts >= beginTimestamp && ts <= endTimestamp;
    }

    public static DateTime ToDateTime(this long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }

    // MouseEnter="@(args => _systemMessage.EndDateTime.ShowTooltipKST(args, TooltipService))"
    //public static void ShowTooltipKST(this DateTime dt, ElementReference elementReference, TooltipService tooltipService) =>
    //    tooltipService.Open(elementReference, $"UTC+9 {dt.ToTimezoneString()}", new TooltipOptions()
    //    {
    //        Position = TooltipPosition.Top,
    //        Delay = 500,
    //        Duration = 4000
    //    });

    public static void ShowMessage(this NotificationService ns, NotificationSeverity severity, string summary, string msg)
    {
        ns.Notify(new NotificationMessage { Severity = severity, Summary = summary, Detail = msg, Duration = 2000 });
    }
}
