namespace AdminManager.Common
{
    public class ScheduleConfig
    {
        public DateTime BeginDateTime;
        public DateTime EndDateTime;
        public int IntervalSeconds = 0;
        public int RepeatCount = 0;
        public bool IsImmediate = false;
    }
}
