using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization;
using System.Runtime.Serialization;
using AdminManager.ModelLog.Shared;

namespace AdminManager.ModelLog
{
    public static class LogCharMapper
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<LogCharModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogCharDataModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });
        }
    }

    public class LogCharModel : LogModelBase
    {
        #nullable enable
        [BsonElement("char")]
        public LogCharDataModel? Char { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogCharDataModel
    {
        [BsonElement("char_id")]
        public int CharId { get; set; }

        [BsonElement("char_lv")]
        public int CharLv { get; set; }

        [BsonElement("cuid")]
        public long CUID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("zone_id")]
        public int ZoneId { get; set; }
    }
}
