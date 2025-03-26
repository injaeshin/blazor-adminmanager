using AdminManager.ModelLog.Shared;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace AdminManager.ModelLog
{
    public static class LogShopMapper
    {
        public static void Init()
        {
            BsonClassMap.RegisterClassMap<LogShopModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);
            });

            BsonClassMap.RegisterClassMap<LogShopDataModel>(classMap =>
            {
                classMap.AutoMap();
            });
        }
    }

    public class LogShopModel : LogModelBase
    {
        [BsonElement("goods")]
        public LogShopDataModel Shop { get; set; }
    }

    [BsonNoId, DataContract]
    public class LogShopDataModel
    {
        [BsonElement("id")]
        public int Id { get; set; }
        [BsonElement("amount")]
        public int Amount { get; set; }
        [BsonElement("cost")]
        public int Cost { get; set; }
    }

}
