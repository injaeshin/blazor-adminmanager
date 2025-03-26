using AdminManager.Common;

namespace AdminManager.ModelView;

public class CharacterModelView : DateTimeModelView
{
    public long AUID { get; set; }

    public long CUID { get; set; }

    public string Name { get; set; }

    public CharStatus Status => DeleteDateTime.GetCharacterStatus();
}

public class CharacterDetailModelView : DateTimeModelView
{
    public long AUID { get; set; }

    public long CUID { get; set; }

    public string Name { get; set; }

    public short Level { get; set; }

    public long LevelExp { get; set; }

    public CharStatus Status => DeleteDateTime.GetCharacterStatus();
}