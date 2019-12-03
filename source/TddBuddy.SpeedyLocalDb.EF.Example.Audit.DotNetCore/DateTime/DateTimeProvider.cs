namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.DateTime
{
    public class DateTimeProvider : IDateTimeProvider
    {
        // prune seconds from DateTime.Now
        public System.DateTime Now => System.DateTime.Parse(System.DateTime.Now.ToString("g"));
    }
}