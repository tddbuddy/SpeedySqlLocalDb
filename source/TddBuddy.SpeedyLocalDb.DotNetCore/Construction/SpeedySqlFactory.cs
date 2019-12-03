namespace TddBuddy.SpeedyLocalDb.DotNetCore.Construction
{
    public static class SpeedySqlFactory
    {
        public static ISpeedySqlLocalDbWrapper CreateWrapper()
        {
            var contextVariables = new ContextVariables();
            return new SpeedySqlLocalDb(contextVariables)
                                       .CreateSpeedyLocalDbWrapper();
        }
    }
}
