using System.Runtime.Serialization;

namespace Situations.Core.Exceptions
{
    [Serializable]
    public class NoActionForRegisteredSituation : Exception
    {
        public NoActionForRegisteredSituation()
        {
        }

        public NoActionForRegisteredSituation(string? message) : base(message)
        {
        }

        public NoActionForRegisteredSituation(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoActionForRegisteredSituation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}