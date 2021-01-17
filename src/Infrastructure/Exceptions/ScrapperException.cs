using System;
using System.Runtime.Serialization;

namespace Infrastructure.Exceptions
{
    [Serializable]
    public class ScrapperException : Exception
    {
        public ScrapperException()
        {
        }

        public ScrapperException(string message) : base(message)
        {
        }

        public ScrapperException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScrapperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}