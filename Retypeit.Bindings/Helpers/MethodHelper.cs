using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Helpers
{
    internal static class MethodHelper
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<string, List<MethodInfo>>> Cache =
            new ConcurrentDictionary<Type, Dictionary<string, List<MethodInfo>>>();

        private static readonly object LockObj = new object();

        public static object Invoke(object target, string methodName, object[] parameters)
        {
            if (!TryInvoke(target, methodName, parameters, out var result))
            {
                if (result is Exception ex)
                    throw ex;

                // If try invoke fails it should always return an exception
                // This is a failsafe..
                throw new TargetInvocationException(
                    $"Failed to invoke [{methodName}] on type [{target.GetType().Name}]", null);
            }

            return result;
        }

        public static bool TryInvoke(object target, string methodName, object[] parameters, out object result)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("message", nameof(methodName));

            var methodsToTry = ListMethodInfo(target, methodName);
            Exception lastException = null;
            foreach (var methodToInvoke in ListMethodInfo(target, methodName))
            {
                if (TryInvokeMethod(target, methodToInvoke, parameters, out result)) return true;

                if (result is Exception ex)
                    lastException = ex;
            }

            if (lastException != null)
                result = lastException;
            else
                result = new TargetInvocationException(CreateErrorMessage(parameters, methodName, methodsToTry), null);
            return false;
        }

        private static bool TryInvokeMethod(object target, MethodInfo methodToInvoke, object[] parameters,
            out object result)
        {
            result = null;
            parameters = parameters ?? new object[] { };
            IEnumerable<object> paramsToInvokeWith = parameters;

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var methodParameters = methodToInvoke.GetParameters();

            if (methodParameters.Length < parameters.Length)
                return false;

            if (methodParameters.Length > parameters.Length)
            {
                //
                // Try to add default parameters
                //
                var paramsWithDefaults = new List<object>(paramsToInvokeWith);
                for (var i = parameters.Length; i < methodParameters.Length; i++)
                {
                    var methodParameter = methodParameters[i];
                    if (!methodParameter.HasDefaultValue)
                        return false;

                    paramsWithDefaults.Add(methodParameter.DefaultValue);
                }

                parameters = paramsWithDefaults.ToArray();
            }

            try
            {
                result = methodToInvoke.Invoke(target, parameters);
                return true;
            }
            catch (TargetInvocationException tiEx)
            {
                // The original exception is wrapped in the TI exception
                result = tiEx.InnerException;
                return false;
            }
            catch (Exception ex)
            {
                result = ex;
                return false;
            }
        }

        private static string CreateErrorMessage(object[] parameters, string methodName,
            IEnumerable<MethodInfo> methodList)
        {
            var errorMessage = new StringBuilder();
            errorMessage.Append($"Failed to execute [{methodName}] with parameters: ");

            errorMessage.Append("[");
            var isFirstParameter = true;
            foreach (var p in parameters)
            {
                if (!isFirstParameter)
                    errorMessage.Append(", ");
                errorMessage.Append(p.GetType().Name);

                isFirstParameter = false;
            }

            errorMessage.Append("]");
            errorMessage.AppendLine("");

            errorMessage.AppendLine("Valid alternatives:");
            foreach (var method in methodList)
            {
                errorMessage.Append($"  * {method.Name}(");
                isFirstParameter = true;
                foreach (var p in method.GetParameters())
                {
                    if (!isFirstParameter)
                        errorMessage.Append(", ");
                    errorMessage.Append($"{p.ParameterType.Name} {p.Name}");

                    isFirstParameter = false;
                }

                errorMessage.Append(")");
            }

            return errorMessage.ToString();
        }

        internal static IEnumerable<MethodInfo> ListMethodInfo(object target, string methodName)
        {
            var methods = Cache.GetOrAdd(target.GetType(), (arg)=>ListMethods(target.GetType()));
            
            if (!methods.TryGetValue(methodName.ToLowerInvariant(), out var methodToInvoke))
                throw new NotSupportedException($"Method {methodName} not supported");

            return methodToInvoke;
        }

        private static Dictionary<string, List<MethodInfo>> ListMethods(Type target)
        {
            // We did not have the type information cached, so cache it now
            var methods = new Dictionary<string, List<MethodInfo>>();

            // Add the different methods that can be invoked by the given methodname
            // Sort them by: parameter count desc
            var methodList = target.GetMethods().Where(i => i.IsPublic && !i.IsStatic);
            foreach (var method in methodList.GroupBy(i => i.Name.ToLowerInvariant()))
                methods.Add(method.Key, method.OrderByDescending(i => i.GetParameters().Count()).ToList());

            return methods;
        }
    }
}