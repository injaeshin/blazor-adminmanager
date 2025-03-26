using AdminManager.Common;

namespace AdminManager.ModelView;

public class AdminMailAttachItem
{
    public int ItemId { get; set; }
    public string ItemStr { get; set; }
    public int MaxAmount { get; set; }
}

public class AdminMailTarget
{
    public long AccountUId { get; set; }
    public long CharUId { get; set; }
    public string CharName { get; set; } = string.Empty;
    public bool Valid { get; set; }
    public bool Send { get; set; }
}

public class AdminMailModelView : DateTimeModelView
{
    public long MUID { get; set; }

    public int MID { get; set; }
    public long AUID { get; set; }

    public long CUID { get; set; }

    public string CharName { get; set; }

    public List<AdminMailAttachItemModelView> AttachItems { get; set; }
}

public class AdminMailAttachItemModelView
{
    public byte Idx { get; set; }

    public byte ItemType { get; set; }

    public int Param { get; set; }

    public int Amount { get; set; }

    public GDT.CurrencyMainType MainType { get; set; }
    
    public GDT.CurrencySubType SubType { get; set; }

    public int ItemId { get; set; }

    public IEnumerable<AdminCurrencySubType> AttachedSubTypeItems = null;
}
