namespace JadeLib.API
{
    using System;

    public static class TimestampUtility
    {
        // Method to create a timestamp in minutes since the Unix epoch
        public static long CreateTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        // Method to convert a DateTime to a timestamp in minutes since the Unix epoch
        public static long ConvertToTimestamp(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeSeconds() / 60;
        }

        // Method to get the current timestamp in minutes since the Unix epoch
        public static long GetCurrentTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60;
        }

        // Method to compare two timestamps
        public static int CompareTimestamps(long timestamp1, long timestamp2)
        {
            if (timestamp1 > timestamp2) return 1;
            if (timestamp1 < timestamp2) return -1;
            return 0;
        }

        // Method to get the difference in minutes between two timestamps
        public static long GetDifference(long timestamp1, long timestamp2)
        {
            return Math.Abs(timestamp1 - timestamp2);
        }
    }
}