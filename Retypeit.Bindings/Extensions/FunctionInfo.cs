using System;
using System.Collections.Generic;
using System.Linq;

namespace Retypeit.Scripts.Bindings.Extensions
{
    public class FunctionInfo
    {
        /// <summary>
        ///     Describes a function in the script language
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        public FunctionInfo(string functionName, params Type[] parameters)
        {
            Name = functionName ?? throw new ArgumentNullException(nameof(functionName));
            Parameters = new List<Type>(parameters);
        }

        /// <summary>
        ///     The functions parameters
        /// </summary>
        public List<Type> Parameters { get; }

        /// <summary>
        ///     Name of the function
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Creates a clone that contains the same data as this instance
        /// </summary>
        /// <returns></returns>
        public FunctionInfo Clone()
        {
            var clone = new FunctionInfo(Name);
            clone.Parameters.AddRange(Parameters);
            return clone;
        }

        /// <summary>
        ///     Validate if two FunctionInfo instances contains the same data
        /// </summary>
        /// <param name="x">Instance 1</param>
        /// <param name="y">Instance 2</param>
        /// <returns></returns>
        public static bool EqualTo(FunctionInfo x, FunctionInfo y)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (!string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase))
                return false;

            if (x.Parameters.Count != y.Parameters.Count)
                return false;

            // Check if parameter list contains the same types in the same order
            return !x.Parameters.Where((parameter, index) => parameter != y.Parameters[index]).Any();
        }
    }
}