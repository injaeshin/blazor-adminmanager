using AdminManager.Model.Shared;
using Dapper.ColumnMapper;
using Dapper;

namespace AdminManager.Model;
public static class AdminMailModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(AdminMailModel), new ColumnTypeMapper(typeof(AdminMailModel)));
        SqlMapper.SetTypeMap(typeof(AdminMailAttachItemModel), new ColumnTypeMapper(typeof(AdminMailAttachItemModel)));
    }
}

public class AdminMailModel : DateTimeModel
{
    [ColumnMapping("mail_uid")]
    public long MUID { get; set; }

    [ColumnMapping("mail_id")]
    public int MID { get; set; }

    [ColumnMapping("auid")]
    public long AUID { get; set; }

    [ColumnMapping("cuid")]
    public long CUID { get; set; }

    [ColumnMapping("char_name")]
    public string CharName { get; set; }

    public List<AdminMailAttachItemModel> AttachItems { get; set; }
}

public class AdminMailAttachItemModel
{
    [ColumnMapping("idx")]
    public byte Idx { get; set; }

    [ColumnMapping("type")]
    public byte ItemType { get; set; }

    [ColumnMapping("param")]
    public int Param { get; set; }

    [ColumnMapping("amount")]
    public int Amount { get; set; }
}
