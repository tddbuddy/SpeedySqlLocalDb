namespace TddBuddy.SpeedySqlLocalDb
{
    public interface ISpeedySqlLocalDb
    {
        ISpeedySqlLocalDbWrapper CreateSpeedyLocalDbWrapper();
    }
}