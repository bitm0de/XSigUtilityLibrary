using System;

namespace XSigUtilityLibrary.Serialization
{
    [Serializable]
    public class XSigSerializationException : Exception
    {
        public XSigSerializationException()
        {
        }

        public XSigSerializationException(string message)
            : base(message)
        {
        }

        public XSigSerializationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}