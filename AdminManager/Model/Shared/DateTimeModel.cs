using Dapper.ColumnMapper;

namespace AdminManager.Model.Shared;
public class DateTimeModel
{
    [ColumnMapping("create_ts")]
    public long CreateTimeStamp { get; set; }

    [ColumnMapping("del_ts")]
    public long DeleteTimeStamp { get; set; }

    [ColumnMapping("expire_ts")]
    public long ExpireTimeStamp { get; set; }

    [ColumnMapping("login_ts")]
    public long LoginTimeStamp { get; set; }

    [ColumnMapping("logout_ts")]
    public long LogoutTimeStamp { get; set; }

    [ColumnMapping("begin_ts")]
    public long BeginTimeStamp { get; set; }

    [ColumnMapping("end_ts")]
    public long EndTimeStamp { get; set; }
}

public class UpdateDateTimeModel
{
    [ColumnMapping("update_ts")]
    public long UpdateTimeStamp { get; set; }
}

public class EventDateTimeModel
{
    [ColumnMapping("begin_ts")]
    public long BeginTimeStamp { get; set; }

    [ColumnMapping("end_ts")]
    public long EndTimeStamp { get; set; }

    [ColumnMapping("update_ts")]
    public long UpdateTimeStamp { get; set; }

    [ColumnMapping("exec_ts")]
    public long ExecuteTimeStamp { get; set; }
}


