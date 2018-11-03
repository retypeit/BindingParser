using System;
using System.Collections.Generic;
using System.Globalization;

namespace Retypeit.Scripts.Bindings.Helpers
{
    /// <summary>
    /// Methods related to how the script manages values
    /// </summary>
    internal static class ValueHelper
    {
        /// <summary>
        ///     Types considered numeric in the script
        /// </summary>
        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int), typeof(double), typeof(decimal),
            typeof(long), typeof(short), typeof(sbyte),
            typeof(byte), typeof(ulong), typeof(ushort),
            typeof(uint), typeof(float)
        };

        /// <summary>
        ///     True if the type will be considered numeric by the script
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNumeric(Type target)
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(target) ?? target);
        }

        /// <summary>
        ///     True if the object will be considered numeric by the script
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNumeric(object target)
        {
            if (target == null)
                return false;

            return IsNumeric(target.GetType());
        }

        /// <summary>
        ///     Converts an object to a string using the provided culture info
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ToString(object value, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));

            if (!(value is string) && value is IConvertible valueConverter)
            {
                if (IsNumeric(value.GetType()))
                    return valueConverter.ToString(culture.NumberFormat);
                if (value is DateTime dtValue) return dtValue.ToString(culture.DateTimeFormat);
            }

            return value.ToString();
        }
    }
}