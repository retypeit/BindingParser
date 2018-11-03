using System;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Helpers;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.ExtensionMethods
{
    internal static class IVisitorExtensions
    {
        /// <summary>
        ///     Calculated two values and returns the result
        /// </summary>
        /// <param name="target">Visitor to use when calculating the values</param>
        /// <param name="value1">Value to the left</param>
        /// <param name="value2">Value to the right</param>
        /// <param name="calcInt">Method to calculate values if int</param>
        /// <param name="calcDec">Method to calculate values if decimal</param>
        /// <param name="error">Method to handle errors (usually type errors)</param>
        /// <returns>The result of the calculation</returns>
        public static object Calculate(this IVisitor target, IAstNode value1, IAstNode value2,
            Func<int, int, object> calcInt, Func<decimal, decimal, object> calcDec,
            Func<object, object, Exception, object> error)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (value1 == null)
                throw new ArgumentNullException(nameof(value1));

            if (value2 == null)
                throw new ArgumentNullException(nameof(value2));

            if (calcInt == null)
                throw new ArgumentNullException(nameof(calcInt));

            if (calcDec == null)
                throw new ArgumentNullException(nameof(calcDec));

            if (error == null)
                throw new ArgumentNullException(nameof(error));

            var v1 = value1.Accept(target);
            var v2 = value2.Accept(target);

            try
            {
                if (v1 is int i1 && v2 is int i2) return calcInt(i1, i2);

                if (!ValueHelper.IsNumeric(v1) || !ValueHelper.IsNumeric(v2))
                {
                    return error(v1, v2, new InvalidOperationException("Value is not numeric!"));
                }

                var d1 = Convert.ToDecimal(v1);
                var d2 = Convert.ToDecimal(v2);
                return calcDec(d1, d2);
            }
            catch (Exception innerException)
            {
                return error(v1, v2, innerException);
            }
        }
    }
}