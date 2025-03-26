using AdminManager.Model.Shared;
using Dapper.ColumnMapper;
using Dapper;

namespace AdminManager.Model;

public static class QuestModelMapper
{
    public static void Init()
    {
        SqlMapper.SetTypeMap(typeof(QuestModel), new ColumnTypeMapper(typeof(QuestModel)));
        SqlMapper.SetTypeMap(typeof(QuestTaskModel), new ColumnTypeMapper(typeof(QuestTaskModel)));
    }
}

public class QuestModel : UpdateDateTimeModel
{
    [ColumnMapping("quest_id")]
    public int QuestID { get; set; }

    [ColumnMapping("quest_state")]
    public byte QuestState { get; set; }
}

public class QuestTaskModel : UpdateDateTimeModel
{
    [ColumnMapping("task_id")]
    public int TaskID { get; set; }

    [ColumnMapping("task_state")]
    public byte TaskState { get; set; }

    [ColumnMapping("task_progress")]
    public int TaskProgress { get; set; }
}
