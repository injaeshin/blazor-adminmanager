using AdminManager.Common;

namespace AdminManager.ModelView;

public class QuestModelView : UpdateDateTimeModelView
{
    public int QuestID { get; set; }
    public byte QuestState { get; set; }

    public IList<QuestTaskModelView> QuestTasks { get; set; }

    public string TypeStr { get; set; }
    public string Name { get; set; }
    public string StatusStr => ((QuestStatus)QuestState).GetDescription();
}

public class QuestTaskModelView : UpdateDateTimeModelView
{
    public int TaskID { get; set; }

    public byte TaskState { get; set; }

    public int TaskProgress { get; set; }

    public string TypeStr { get; set; }
    public string Name { get; set; }
    public string StatusStr => ((QuestStatus)TaskState).GetDescription();
}