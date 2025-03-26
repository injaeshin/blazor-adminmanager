using AdminManager.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminManager.ModelView;

public class LogCurrencyDataModelView
{
    public byte Type { get; set; }
    public int Amount { get; set; }
    public int PrevAmount { get; set; }
    public int RemainAmount { get; set; }

    public string FilterStr { get; set; }
    public string ReasonStr { get; set; }
    public string SubTypeStr { get; set; } = default!;

    public DateTime LogDateTime { get; set; }
    public string LogDateTimeStr => LogDateTime.ToAWFormat();
}

public class LogTierDataModelView
{
    public byte Type { get; set; }

    public int PrevExp { get; set; }
    public int RemainExp { get; set; }
    public int PrevGrade { get; set; }
    public int RemainGrade { get; set; }

    public int Exp => RemainExp - PrevExp;

    public string FilterStr { get; set; }
    public string ReasonStr { get; set; }

    public DateTime LogDateTime { get; set; }
    public string LogDateTimeStr => LogDateTime.ToAWFormat();
}
public class LogMailDataModelView
{
    public long Uid { get; set; }
    public int Id { get; set; }
    public int Owner { get; set; }
    public int State { get; set; }
    public int ExpireTs { get; set; }

    public string FilterStr { get; set; }
    public string ReasonStr { get; set; }

    public DateTime LogDateTime { get; set; }
    public string LogDateTimeStr => LogDateTime.ToAWFormat();
}

public class LogShopDataModelView
{
    public int Id { get; set; }
    public string IdStr { get; set; }
    public int Amount { get; set; }
    public int Cost { get; set; }

    public string FilterStr { get; set; }
    public string ReasonStr { get; set; }
    public DateTime LogDateTime { get; set; }

    public string LogDateTimeStr => LogDateTime.ToAWFormat();
}

public class LogCharDataModelView
{
    public int CharId { get; set; }

    public int CharLv { get; set; }

    public long CUID { get; set; }

    public string Name { get; set; }

    public int ZoneId { get; set; }

    public string FilterStr { get; set; }
    public string ReasonStr { get; set; }

    public DateTime LogDateTime { get; set; }

    public string LogDateTimeStr => LogDateTime.ToAWFormat();
}
