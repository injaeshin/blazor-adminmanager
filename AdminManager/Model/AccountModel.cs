using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;

public static class AccountModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(AccountModel), new ColumnTypeMapper(typeof(AccountModel)));
        SqlMapper.SetTypeMap(typeof(AccountTierModel), new ColumnTypeMapper(typeof(AccountTierModel)));
    }
}

public class AccountModel : DateTimeModel
{
    [ColumnMapping("account_uid")]
    public long AUID { get; set; }
    [ColumnMapping("cs_code")]
    public string CSCode { get; set; }
    [ColumnMapping("tier_lv")]
    public short TierLv { get; set; }
    [ColumnMapping("tier_exp")]
    public long TierExp { get; set; }

    [ColumnMapping("bank_user_id")]
    public int BankUserId { get; set; }
    [ColumnMapping("spend_user_id")]
    public int SpendingUserId { get; set; }
    [ColumnMapping("wallet_address")]
    public string WalletAddress { get; set; }
    [ColumnMapping("platform")]
    public string Platform { get; set; }
    [ColumnMapping("privilege")]
    public string Privilege { get; set; }
}

public class AccountTierModel
{
    [ColumnMapping("tier_lv")]
    public short TierLv { get; set; }
    [ColumnMapping("tier_exp")]
    public long TierExp { get; set; }
}

