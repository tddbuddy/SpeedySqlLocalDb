using System;

namespace TddBuddy.SpeedySqlLocalDb.EF.Examples.ExampleDb
{
    public class DateTimeProvider : IDateTimeProvider
    {
        // prune seconds from DateTime.Now
        public DateTime Now => DateTime.Parse(DateTime.Now.ToString("g"));
    }
}