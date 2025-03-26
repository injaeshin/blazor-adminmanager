using AdminManager.Common;
using AdminManager.Model.Shared;
using Dapper;
using Dapper.ColumnMapper;

namespace AdminManager.Model
{
    public static class EventModelMapper
    {
        public static void Init()
        {
            SqlMapper.SetTypeMap(typeof(AirdropEventModel), new ColumnTypeMapper(typeof(AirdropEventModel)));
        }
    }

    public class AirdropEventModel : EventDateTimeModel
    {
        [ColumnMapping("no")]
        public int No { get; set; }

        [ColumnMapping("game_type")]
        public GameType EventGameType { get; set; }

        [ColumnMapping("use_amount")]
        public int UseAmount { get; set; }

        [ColumnMapping("require_amount")]
        public int RequireAmount { get; set; }

        [ColumnMapping("exec_ret")]
        public EventResultType ExecuteRet { get; set; }

        [ColumnMapping("exec_msg")]
        public string ExecuteMsg { get; set; }

        [ColumnMapping("note")]
        public string Note { get; set; }
    }
}
