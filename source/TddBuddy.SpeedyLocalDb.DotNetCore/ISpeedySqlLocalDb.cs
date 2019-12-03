namespace TddBuddy.SpeedyLocalDb.DotNetCore
{
    public interface ISpeedySqlLocalDb
    {
        ISpeedySqlLocalDbWrapper CreateSpeedyLocalDbWrapper();
    }
}