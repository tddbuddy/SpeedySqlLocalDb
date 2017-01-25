namespace TddBuddy.SpeedySqlLocalDb.Construction
{
    public class SpeedySqlBuilder
    {
        private int _transactionTimeoutMinutes;

        public SpeedySqlBuilder()
        {
            _transactionTimeoutMinutes = 1;
        }

        public SpeedySqlBuilder WithTransactionTimeout(int timeout)
        {
            _transactionTimeoutMinutes = timeout;
            return this;
        }

        public ISpeedySqlLocalDbWrapper BuildWrapper()
        {
            var contextVariables = new ContextVariables
            {
                TransactionTimeoutMinutes = _transactionTimeoutMinutes
            };

            return new SpeedySqlLocalDb(contextVariables).CreateSpeedyLocalDbWrapper();
        }
    }
}
