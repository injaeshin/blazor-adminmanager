using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace AdminManager.ModelLog.Shared
{
    public static class LogModelMapper
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<LogCharacterModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogModelBase>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogCurrencyModelBase>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogItemModelBase>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });
        }
    }

    //public class LogModel
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public ObjectId Id { get; set; }
    //    [BsonElement("cate")]
    //    public int Cate { get; set; }
    //    [BsonElement("filter")]
    //    public int Filter { get; set; }
    //    [BsonElement("reason")]
    //    public int Reason { get; set; }
    //    [BsonElement("server_id")]
    //    public int ServerId { get; set; }
    //    [BsonElement("time")]
    //    public DateTime RegDateTime { get; set; }
    //}

    public class LogModelBase
    {
        //[BsonId]
        //public ObjectId _id { get; set; }

        [BsonElement("cate")]
        public int Cate { get; set; }

        [BsonElement("filter")]
        public int Filter { get; set; }

        [BsonElement("reason")]
        public int Reason { get; set; }

        [BsonElement("server_id")]
        public int ServerId { get; set; }

        [BsonElement("time")]
        public DateTime RegDateTime { get; set; }

        /////////////////////////////////////////////////

        [BsonElement("user")]
        public LogCharacterModel User { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogCharacterModel
    {
        [BsonElement("auth_key")]
        public string AuthKey { get; set; } = string.Empty;

        [BsonElement("auid")]
        public long AccountUID { get; set; }

        [BsonElement("cuid")]
        public long CharUID { get; set; }

        [BsonElement("tier")]
        public int Tier { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogCurrencyModelBase
    {
        [BsonElement("amount")]
        public int Amount { get; set; }
        [BsonElement("sub_type")]
        public byte SubType { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogItemModelBase
    {
        [BsonElement("id")]
        public int ItemId { get; set; }
        [BsonElement("uid")]
        public int Uid { get; set; }
        [BsonElement("amount")]
        public int Amount { get; set; }
        [BsonElement("storage")]
        public int Storage { get; set; }
    }

    //public class AccountLogModel : LogModel
    //{
    //    [BsonElement("auth_key")]
    //    public string AuthKey { get; set; } = string.Empty;
    //    [BsonElement("auid")]
    //    public long AccountUID { get; set; }
    //    [BsonElement("cuid")]
    //    public long CharUID { get; set; }
    //    [BsonElement("tier")]
    //    public int Tier { get; set; }
    //}

}
