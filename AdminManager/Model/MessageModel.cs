using AdminManager.Common;
using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;
public static class MessageModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(MessageModel), new ColumnTypeMapper(typeof(MessageModel)));
        SqlMapper.SetTypeMap(typeof(SystemMessageModel), new ColumnTypeMapper(typeof(SystemMessageModel)));
    }
}

public class MessageModel
{
    [ColumnMapping("no")]
    public int No { get; set; }

    [ColumnMapping("language")]
    public GDT.LanguageType Language { get; set; } = GDT.LanguageType.LT_Korean;

    [ColumnMapping("message")]
    public string Message { get; set; } = string.Empty;
}

public class SystemMessageModel : DateTimeModel
{
    [ColumnMapping("no")]
    public int No { get; set; }

    public MessageType MessageType { get; set; }

    [ColumnMapping("interval_min")]
    public MessageIntervalType Interval { get; set; }
}

