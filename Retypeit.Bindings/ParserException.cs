using System;
using System.Runtime.Serialization;
using Retypeit.Scripts.Bindings.Parser;

namespace Retypeit.Scripts.Bindings.Exceptions
{
    [Serializable]
    public class ParserException : Exception
    {
        public ParserException(TokenStream stream)
        {
            TokenTrace = stream.ToString();
        }

        public ParserException(string message, TokenStream stream) : base(message)
        {
            TokenTrace = stream.ToString();
        }

        public ParserException(string message, TokenStream stream, Exception inner) : base(message, inner)
        {
            TokenTrace = stream.ToString();
        }

        protected ParserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public string TokenTrace { get; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return $"{TokenTrace}\n\n{StackTrace}";

            return $"{Message}\n\n{TokenTrace}\n\n{StackTrace}";
        }
    }
}