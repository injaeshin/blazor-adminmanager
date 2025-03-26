using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;

public static class ItemModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(ItemModel), new ColumnTypeMapper(typeof(ItemModel)));
    }
}

public class ItemModel : UpdateDateTimeModel
{
    [ColumnMapping("item_uid")]
    public long ItemUID { get; set; }

    [ColumnMapping("item_id")]
    public int ItemID { get; set; }

    [ColumnMapping("storage_type")]
    public string StorageType { get; set; }

    [ColumnMapping("equip_slot")]
    public string EquipSlot { get; set; }

    [ColumnMapping("amount")]
    public int Amount { get; set; }
}

