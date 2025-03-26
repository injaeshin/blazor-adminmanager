using AdminManager.ModelLog.Shared;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminManager.ModelLog;

public static class LogCurrencyMapper
{
    public static void Init()
    {
        BsonClassMap.RegisterClassMap<LogCurrencyModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
        });

        BsonClassMap.RegisterClassMap<LogCurrencyDataModel>(classMap =>
        {
            classMap.AutoMap();
        });
    }
}

public class LogCurrencyModel : LogModelBase
{
    [BsonElement("currency")]
    public List<LogCurrencyDataModel> Currency { get; set; } = null;
}

public class LogCurrencyDataModel
{
    [BsonElement("type")]
    public byte Type { get; set; }

    [BsonElement("amount")]
    public int Amount { get; set; }

    [BsonElement("prev")]
    public int PrevAmount { get; set; }

    [BsonElement("remain")]
    public int RemainAmount { get; set; }
}
