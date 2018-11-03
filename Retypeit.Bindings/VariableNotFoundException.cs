using System;
using System.Runtime.Serialization;

namespace Retypeit.Scripts.Bindings.Exceptions
{
    class VariableNotFoundException : Exception
    {
        public string VariableName { get; set; }

        public VariableNotFoundException(string variableName)
        {
            VariableName = variableName;
        }

        public VariableNotFoundException(string variableName, string message) : base(message)
        {
            VariableName = variableName;
        }

        public VariableNotFoundException(string variableName, string message, Exception innerException) : base(message, innerException)
        {
            VariableName = variableName;
        }

        protected VariableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
