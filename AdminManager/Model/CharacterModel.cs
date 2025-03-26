using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model;

public static class CharacterModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(CharacterModel), new ColumnTypeMapper(typeof(CharacterModel)));
        SqlMapper.SetTypeMap(typeof(CharacterDetailModel), new ColumnTypeMapper(typeof(CharacterDetailModel)));
    }
}

public class CharacterModel : DateTimeModel
{
    [ColumnMapping("auid")]
    public long AUID { get; set; }

    [ColumnMapping("cuid")]
    public long CUID { get; set; }

    [ColumnMapping("name")]
    public string Name { get; set; }
}

public class CharacterDetailModel : DateTimeModel
{
    [ColumnMapping("auid")]
    public long AUID { get; set; }

    [ColumnMapping("cuid")]
    public long CUID { get; set; }

    [ColumnMapping("name")]
    public string Name { get; set; }

    [ColumnMapping("lv")]
    public short Level { get; set; }

    [ColumnMapping("lv_exp")]
    public long LevelExp { get; set; }
}
