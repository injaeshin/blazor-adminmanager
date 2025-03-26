using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;

public static class CurrencyModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(CurrencyModel), new ColumnTypeMapper(typeof(CurrencyModel)));
    }
}

public class CurrencyModel : UpdateDateTimeModel
{
    [ColumnMapping("sub_type")]
    public short SubType { get; set; }

    [ColumnMapping("default_amount")]
    public int DefaultAmount { get; set; }

    [ColumnMapping("current_amount")]
    public int CurrentAmount { get; set; }
}
