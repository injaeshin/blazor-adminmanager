using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;

public static class MailModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(MailModel), new ColumnTypeMapper(typeof(MailModel)));
        SqlMapper.SetTypeMap(typeof(MailAttachItemModel), new ColumnTypeMapper(typeof(MailAttachItemModel)));
    }
}

public class MailModel : DateTimeModel
{
    [ColumnMapping("mail_uid")] public long MUID { get; set; }

    [ColumnMapping("mail_id")] public long MailID { get; set; }

    [ColumnMapping("mail_state")] public byte MailState { get; set; }

    [ColumnMapping("is_account")] public bool IsAccount { get; set; }
}

public class MailAttachItemModel
{
    [ColumnMapping("idx")] public byte Idx { get; set; }

    [ColumnMapping("type")] public byte ItemType { get; set; }

    [ColumnMapping("param")] public int Param { get; set; }

    [ColumnMapping("amount")] public int Amount { get; set; }
}