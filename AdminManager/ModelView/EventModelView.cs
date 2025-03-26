using AdminManager.Common;
using AdminManager.Service;

namespace AdminManager.ModelView
{
    public class EventGameType
    {
        public string TypeStr { get; set; }
        public GameType Type { get; set; }
    }

    public class AirdropEventModelView() : EventDateTimeModelView
    {
        public int No { get; set; }

        public GameType EventGameType { get; set; }

        public int UseAmount { get; set; }

        public int RequireAmount { get; set; }

        public EventResultType ExecuteRet { get; set; }

        public string ExecuteRetStr => ExecuteRet.GetDescription();

        public string ExecuteMsg { get; set; }

        public string Note { get; set; }

        public static AirdropEventModelView New()
        {
            return new AirdropEventModelView()
            {
                No = 0,
                EventGameType = GameType.None,
                UseAmount = 0,
                RequireAmount = 0,
                Note = string.Empty,
                BeginDateTime = DateTime.Now,
            };
        }
    }
}
