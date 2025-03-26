using AdminManager.Common;
using GDT;

namespace AdminManager.ModelView;

public class MessageModelView
{
    public int No { get; set; }
    
    public GDT.LanguageType Language { get; set; } = GDT.LanguageType.LT_Korean;
 
    public string Message { get; set; } = string.Empty;

    public static MessageModelView New(int nextNo, GDT.LanguageType type = LanguageType.LT_English, string msg = "")
    {
        return new MessageModelView()
        {
            No = nextNo,
            Language = type,
            Message = msg
        };
    }
}

public class SystemMessageModelView : DateTimeModelView
{
    public int No { get; set; }
    public MessageType MessageType { get; set; }

    public MessageIntervalType Interval { get; set; }

    public GDT.LanguageType DefaultLanguage { get; set; } = GDT.LanguageType.LT_English;

    public static SystemMessageModelView New()
    {
        return new SystemMessageModelView()
        {
            No = 0,
            BeginDateTime = DateTime.Now,
            EndDateTime = DateTime.Now.AddHours(1),
            Interval = MessageIntervalType.None
        };
    }

    public ProcessStatus Status => GetStatus();

    private ProcessStatus GetStatus()
    {
        var now = DateTime.Now;
        if (BeginDateTime > now)
            return ProcessStatus.Wait;

        if (BeginDateTime <= now && EndDateTime >= now)
            return ProcessStatus.Progress;

        return ProcessStatus.Expired;
    }
}
