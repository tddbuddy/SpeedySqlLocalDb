namespace TddBuddy.SpeedyLocalDb.EF.Example.Audit.DotNetCore.DateTime
{
    public interface IDateTimeProvider  
    {
        System.DateTime Now { get; }
    }
}