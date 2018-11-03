using System;
using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Extensions;

namespace Retypeit.Scripts.Bindings.Interpreter
{
    /// <summary>
    ///     Class that is used to chain function runners => setup a middleware pattern.
    /// </summary>
    internal class FunctionRunnerWrapper
    {
        /// <summary>
        ///     runner that this class wraps
        /// </summary>
        private readonly IFunctionRunner _source;

        /// <summary>
        ///     Wrapper class used to implement the middleware pattern
        /// </summary>
        /// <param name="source"></param>
        /// <param name="next"></param>
        public FunctionRunnerWrapper(IFunctionRunner source, FunctionRunnerWrapper next)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            Next = next;
        }

        /// <summary>
        ///     Next function in the chain to run
        /// </summary>
        /// <value></value>
        public FunctionRunnerWrapper Next { get; }

        public object Invoke(FunctionContext context)
        {
            var nextFunctionInTheChain = Next == null ? null : new Func<object>(() => Next.Invoke(context));
            return _source.Invoke(context, nextFunctionInTheChain);
        }

        public ICollection<FunctionInfo> ListSupportedFunctions()
        {
            var next = Next == null ? null : new Func<ICollection<FunctionInfo>>(() => Next.ListSupportedFunctions());
            return _source.ListSupportedFunctions(next);
        }
    }
}