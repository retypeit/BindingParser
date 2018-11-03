using System;

namespace Retypeit.Scripts.Bindings
{
    public class BindingResolverException : Exception
    {
        public BindingResolverException()
        {
        }

        public BindingResolverException(string message) : base(message)
        {
        }

        public BindingResolverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
