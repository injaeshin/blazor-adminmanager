using AdminManager.ModelLog.Shared;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminManager.ModelLog
{
    public static class LogTierMapper
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<LogTierModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogTierDataModel>(classMap =>
            {
                classMap.AutoMap();
            });
        }
    }

    public class LogTierModel : LogModelBase
    {
        [BsonElement("tier")]
        public LogTierDataModel Tier { get; set; }
    }

    public class LogTierDataModel
    {
        [BsonElement("type")]
        public byte Type { get; set; }

        [BsonElement("prev_exp")]
        public int PrevExp { get; set; }

        [BsonElement("remain_exp")]
        public int RemainExp { get; set; }

        [BsonElement("prev_grade")]
        public int PrevGrade { get; set; }

        [BsonElement("remain_grade")]
        public int RemainGrade { get; set; }
    }
}
