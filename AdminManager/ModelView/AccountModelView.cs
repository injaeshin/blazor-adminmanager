namespace AdminManager.ModelView;

public class AccountModelView : DateTimeModelView
{
    public long AUID { get; set; }
    public string CSCode { get; set; }
    public short TierLv { get; set; }
    public long TierExp { get; set; }

    public int BankUserId { get; set; }
    public int SpendingUserId { get; set; }
    public string WalletAddress { get; set; }
    public string Platform { get; set; }
    public string Privilege { get; set; }
}

public class AccountTierModelView
{
    public short TierLv { get; set; }
    public long TierExp { get; set; }
}