using System;
using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Extensions
{
    /// <summary>
    ///     Class tasked with running functions within the script
    /// </summary>
    public interface IFunctionRunner
    {
        /// <summary>
        ///     Called when the script wants to invoke a function
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        object Invoke(FunctionContext context, Func<object> next);

        /// <summary>
        ///     Lists all valid function of this function runner.
        ///     This is used when parsing the script.
        /// </summary>
        /// <returns></returns>
        ICollection<FunctionInfo> ListSupportedFunctions(Func<ICollection<FunctionInfo>> next);
    }
}