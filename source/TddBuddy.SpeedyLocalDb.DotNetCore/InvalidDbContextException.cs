using System;
using System.Runtime.Serialization;

namespace TddBuddy.SpeedyLocalDb.DotNetCore
{
    [Serializable]
    internal class InvalidDbContextException : Exception
    {
        public InvalidDbContextException()
        {
        }

        public InvalidDbContextException(string message) : base(message)
        {
        }

        public InvalidDbContextException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDbContextException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}