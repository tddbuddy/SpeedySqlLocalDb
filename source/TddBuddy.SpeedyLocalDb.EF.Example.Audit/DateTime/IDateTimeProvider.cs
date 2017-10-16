namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DateTime
{
    public interface IDateTimeProvider  
    {
        System.DateTime Now { get; }
    }
}