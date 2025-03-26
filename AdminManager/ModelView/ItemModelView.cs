namespace AdminManager.ModelView;

public class ItemModelView : UpdateDateTimeModelView
{
    public long ItemUID { get; set; }

    public int ItemID { get; set; }

    public string StorageType { get; set; }

    public string EquipSlot { get; set; }

    public int Amount { get; set; }

    public string ItemName { get; set; } = default!;
}