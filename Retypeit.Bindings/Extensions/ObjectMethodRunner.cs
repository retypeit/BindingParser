using System;
using System.Collections.Generic;
using System.Linq;
using Retypeit.Scripts.Bindings.Helpers;

namespace Retypeit.Scripts.Bindings.Extensions
{
    /// <summary>
    ///     Used by the script language to invoke methods on the provided instance
    /// </summary>
    public class ObjectMethodRunner : IFunctionRunner
    {
        private readonly List<FunctionInfo> _supportedFunctions;
        private object _target;
        /// <summary>
        ///     Used by the script language to invoke methods on the provided instnace
        /// </summary>
        /// <param name="target"></param>
        public ObjectMethodRunner(object target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _supportedFunctions = ListSupportedFunctions(target);
        }

        public object Invoke(FunctionContext context, Func<object> next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // Will try to find any public method who´s name and parameter list matches the context values.
            if (next == null) return MethodHelper.Invoke(_target, context.Name, context.Parameters.ToArray());

            // Another middleware found, if we cant handle the request.. simply pass it to the next middleware
            if (MethodHelper.TryInvoke(_target, context.Name, context.Parameters.ToArray(), out var obj)) return obj;

            return next.Invoke();
        }

        /// <summary>
        ///     List functions that is supported
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public ICollection<FunctionInfo> ListSupportedFunctions(Func<ICollection<FunctionInfo>> next)
        {
            var output = new List<FunctionInfo>(_supportedFunctions);

            var supportedFunctionsByNextMiddleware = next?.Invoke();
            if (supportedFunctionsByNextMiddleware != null)
                foreach (var funcToAdd in supportedFunctionsByNextMiddleware)
                    if (!output.Any(existingFunc => FunctionInfo.EqualTo(existingFunc, funcToAdd)))
                        output.Add(funcToAdd);

            return output;
        }

        /// <summary>
        ///     List the functions that will be supported on the target type
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private List<FunctionInfo> ListSupportedFunctions(object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            var supportedFunctions = new List<FunctionInfo>();
            // Map all functions and their varius parameter combinations
            foreach (var method in target.GetType().GetMethods().Where(i => i.IsPublic))
            {
                var funcInfo = new FunctionInfo(method.Name);
                foreach (var param in method.GetParameters())
                {
                    if (param.IsOptional) supportedFunctions.Add(funcInfo.Clone());

                    funcInfo.Parameters.Add(param.ParameterType);
                }

                supportedFunctions.Add(funcInfo);
            }

            return supportedFunctions;
        }
    }
}