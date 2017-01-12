namespace TddBuddy.SpeedySqlLocalDb
{
    public class SpeedySqlFactory
    {
        public ISpeedySqlLocalDbWrapper CreateWrapper()
        {
            var contextVariables = new ContextVariables();
            return new SpeedySqlLocalDb(contextVariables).CreateSpeedyLocalDbWrapper();
        }
    }
}
