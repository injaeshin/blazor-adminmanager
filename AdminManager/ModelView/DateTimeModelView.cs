using AdminManager.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AdminManager.ModelView;

public class DateTimeModelView
{
    private long _createTimeStamp;
    private long _deleteTimeStamp;
    private long _expireTimeStamp;
    private long _loginTimeStamp;
    private long _logoutTimeStamp;
    private long _beginTimeStamp;
    private long _endTimeStamp;

    private DateTime _createDateTime;
    private DateTime _deleteDateTime;
    private DateTime _expireDateTime;
    private DateTime _loginDateTime;
    private DateTime _logoutDateTime;
    private DateTime _beginDateTime;
    private DateTime _endDateTime;

    public long CreateTimeStamp
    {
        get => _createTimeStamp;
        set
        {
            _createTimeStamp = value;
            _createDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long DeleteTimeStamp
    {
        get => _deleteTimeStamp;
        set
        {
            _deleteTimeStamp = value;
            _deleteDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long ExpireTimeStamp
    {
        get => _expireTimeStamp;
        set
        {
            _expireTimeStamp = value;
            _expireDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long LoginTimeStamp
    {
        get => _loginTimeStamp;
        set
        {
            _loginTimeStamp = value;
            _loginDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long LogoutTimeStamp
    {
        get => _logoutTimeStamp;
        set
        {
            _logoutTimeStamp = value;
            _logoutDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long BeginTimeStamp
    {
        get => _beginTimeStamp;
        set
        {
            _beginTimeStamp = value;
            _beginDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long EndTimeStamp
    {
        get => _endTimeStamp;
        set
        {
            _endTimeStamp = value;
            _endDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public DateTime CreateDateTime
    {
        get => _createDateTime;
        set
        {
            _createDateTime = value;
            _createTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime DeleteDateTime
    {
        get => _deleteDateTime;
        set
        {
            _deleteDateTime = value;
            _deleteTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime ExpireDateTime
    {
        get => _expireDateTime;
        set
        {
            _expireDateTime = value;
            _expireTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime LoginDateTime
    {
        get => _loginDateTime;
        set
        {
            _loginDateTime = value;
            _loginTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime LogoutDateTime
    {
        get => _logoutDateTime;
        set
        {
            _logoutDateTime = value;
            _logoutTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime BeginDateTime
    {
        get => _beginDateTime;
        set
        {
            _beginDateTime = value;
            _beginTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime EndDateTime
    {
        get => _endDateTime;
        set
        {
            _endDateTime = value;
            _endTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }
    
    public string CreateDT => CreateDateTime.ToAWFormat();
    public string DeleteDT => (DeleteDateTime == Utility.EmptyDateTime) ? "-" : DeleteDateTime.ToAWFormat();
    public string ExpireDT => (ExpireDateTime == Utility.EmptyDateTime) ? "-" : ExpireDateTime.ToAWFormat();
    public string LoginDT => LoginDateTime.ToAWFormat();
    public string LogoutDT => (LogoutDateTime == Utility.EmptyDateTime) ? "-" : LogoutDateTime.ToAWFormat();
    public string BeginDT => BeginDateTime.ToAWFormat();
    public string EndDT => EndDateTime.ToAWFormat();
    
    #region BeginDateTime / EndDateTime (within client)

    public DateTime BeginDate
    {
        get => BeginDateTime.Date;
        set => BeginDateTime = value + BeginDateTime.TimeOfDay;
    }

    public DateTime BeginTime
    {
        get => BeginDateTime;
        set => BeginDateTime = BeginDateTime.Date.Add(value.TimeOfDay);
    }
    public DateTime EndDate
    {
        get => EndDateTime.Date;
        set => EndDateTime = value + EndDateTime.TimeOfDay;
    }
    public DateTime EndTime
    {
        get => EndDateTime;
        set => EndDateTime = EndDateTime.Date.Add(value.TimeOfDay);
    }

    #endregion
}

public class UpdateDateTimeModelView
{ 
    private long _updateTimeStamp;

    private DateTime _updateDateTime;

    public long UpdateTimeStamp
    {
        get => _updateTimeStamp;
        set
        {
            _updateTimeStamp = value;
            _updateDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public DateTime UpdateDateTime
    {
        get => _updateDateTime;
        set
        {
            _updateDateTime = value;
            _updateTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public string UpdateDT => UpdateDateTime.ToAWFormat();
}

public class EventDateTimeModelView
{
    private long _beginTimeStamp;
    private long _endTimeStamp;
    private long _updateTimeStamp;
    private long _executeTimeStamp;

    private DateTime _beginDateTime;
    private DateTime _endDateTime;
    private DateTime _updateDateTime;
    private DateTime _executeDateTime;

    public long BeginTimeStamp
    {
        get => _beginTimeStamp;
        set
        {
            _beginTimeStamp = value;
            _beginDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long EndTimeStamp
    {
        get => _endTimeStamp;
        set
        {
            _endTimeStamp = value;
            _endDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long UpdateTimeStamp
    {
        get => _updateTimeStamp;
        set
        {
            _updateTimeStamp = value;
            _updateDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public long ExecuteTimeStamp
    {
        get => _executeTimeStamp;
        set
        {
            _executeTimeStamp = value;
            _executeDateTime = DateTimeOffset.FromUnixTimeSeconds(value).UtcDateTime.ToTimeZone();
        }
    }

    public DateTime BeginDateTime
    {
        get => _beginDateTime;
        set
        {
            _beginDateTime = value;
            _beginTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime EndDateTime
    {
        get => _endDateTime;
        set
        {
            _endDateTime = value;
            _endTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime UpdateDateTime
    {
        get => _updateDateTime;
        set
        {
            _updateDateTime = value;
            _updateTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public DateTime ExecuteDateTime
    {
        get => _executeDateTime;
        set
        {
            _executeDateTime = value;
            _executeTimeStamp = ((DateTimeOffset)value).ToUnixTimeSeconds();
        }
    }

    public string BeginDT => BeginDateTime.ToAWFormat();
    public string EndDT => EndDateTime.ToAWFormat();
    public string UpdateDT => UpdateDateTime.ToAWFormat();
    public string ExecuteDT => (ExecuteDateTime.AddHours(9) == Utility.EmptyDateTime) ? "-" : ExecuteDateTime.ToAWFormat();

    public DateTime BeginDate
    {
        get => BeginDateTime.Date;
        set => BeginDateTime = value + BeginDateTime.TimeOfDay;
    }
    public DateTime BeginTime
    {
        get => BeginDateTime;
        set => BeginDateTime = BeginDateTime.Date.Add(value.TimeOfDay);
    }
    public DateTime EndDate
    {
        get => EndDateTime.Date;
        set => EndDateTime = value + EndDateTime.TimeOfDay;
    }
    public DateTime EndTime
    {
        get => EndDateTime;
        set => EndDateTime = EndDateTime.Date.Add(value.TimeOfDay);
    }
}

