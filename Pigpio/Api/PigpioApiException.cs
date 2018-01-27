using System;
using System.Runtime.Serialization;

namespace Pigpio.Api
{
    [Serializable]
    public class PigpioApiException : Exception
    {
        public PigpioApiException()
            : base()
        {
        }

        public PigpioApiException(string message)
            : base(message)
        {
        }

        public PigpioApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PigpioApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
