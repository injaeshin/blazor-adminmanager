using AdminManager.Common;

namespace AdminManager.ModelView;

public class MailHeader
{
    public int Id { get; set; }
    public string Title { get; set; }
}

public class MailBody
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string Name { get; set; }
    public int Term { get; set; }
    public string KeepType { get; set; }
    public GDT.ApplicationType AccountType { get; set; }
}

public class MailModelView : DateTimeModelView
{
    public long MUID { get; set; }
    public long MailID { get; set; }
    public byte MailState { get; set; }
    public bool IsAccount { get; set; }

    public string MailStateStr => ((MailState)MailState).GetDescription();

    public IList<MailAttachItemModelView> AttachItems { get; set; }
}

public class MailAttachItemModelView
{
    public byte Idx { get; set; }
    public byte ItemType { get; set; }
    public int Param { get; set; }
    public int Amount { get; set; }

    public string ItemTypeStr { get; set; }
    public string ItemName { get; set; }
}
