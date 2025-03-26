using AdminManager.ModelLog.Shared;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace AdminManager.ModelLog
{
    public static class LogMailMapper
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<LogMailModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogMailDataModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogMailAttachModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });
        }
    }

    public class LogMailModel : LogModelBase
    {
        [BsonElement("mail")]
        public LogMailDataModel Mail { get; set; }

        [BsonElement("attach")]
        public LogMailAttachModel Attach { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogMailDataModel
    {
        [BsonElement("uid")]
        public long Uid { get; set; }
        [BsonElement("id")]
        public int Id { get; set; }
        [BsonElement("owner")]
        public int Owner { get; set; }
        [BsonElement("state")]
        public int State { get; set; }
        [BsonElement("expire_ts")]
        public int ExpireTs { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogMailAttachModel
    {
        #nullable enable
        [BsonElement("currency")]
        private List<LogCurrencyModelBase>? Currency { get; set; }
        [BsonElement("coin")]
        private List<LogCurrencyModelBase>? Coin { get; set; }
        [BsonElement("item")]
        private List<LogItemModelBase>? Item { get; set; }
    }
}
