using System.Collections;

namespace AdminManager.Common
{
    public class Utility
    {
        public static int TimeOffset = 9;
        //public static int TimeOffsetMinutes = TimeOffset * 60;
        //public static int TimeOffsetSeconds = TimeOffsetMinutes * 60;

        public static long GetTimeStamp() => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        public static DateTime EmptyDateTime = new(1970, 1, 1, 9/*+9*/, 0, 0, DateTimeKind.Utc);

        public static bool IsNullOrEmpty<T>(T input) where T : class
        {
            if (input == null)
            {
                return true;
            }

            if (input is IEnumerable enumerable)
            {
                return !enumerable.Cast<object>().Any();
            }

            // input is not a collection, so it's not "empty"
            return false;
        }

        public static bool IsNullOrEmpty<T1, T2>(T1 input1, T2 input2) where T1 : class where T2 : class
        {
            return IsNullOrEmpty(input1) || IsNullOrEmpty(input2);
        }
    }
}
