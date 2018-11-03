using System;
using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Extensions
{
    public class FunctionContext
    {
        /// <summary>
        ///     Context object used when running a function
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public FunctionContext(string name, List<object> parameters)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"{nameof(name)} must not be null or empty", nameof(name));

            Name = name;
            Parameters = parameters ?? new List<object>();
        }

        /// <summary>
        ///     Name of the function to run
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Parameters to use with the function
        /// </summary>
        public List<object> Parameters { get; }
    }
}