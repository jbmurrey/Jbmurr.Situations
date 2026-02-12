using System.Runtime.Serialization;

namespace Situations.Core.Exceptions
{
    [Serializable]
    public class UnregisteredSituationException : Exception
    {
        public UnregisteredSituationException()
        {
        }

        public UnregisteredSituationException(string? message) : base(message)
        {
        }

        public UnregisteredSituationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnregisteredSituationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static void ThrowIf(bool condition, string message)
        {
            if (condition)
            {
                throw new UnregisteredSituationException(message);
            }
        }
    }
}