using System;
using System.Globalization;

namespace Retypeit.Scripts.Bindings.Interpreter
{
    /// <summary>
    ///     Functions that can be used from within the script
    ///     simply create more public methods here to add new functions
    /// </summary>
    public class ScriptFunctions
    {
        private readonly Func<CultureInfo> _cultureInfoProvider;

        public ScriptFunctions(Func<CultureInfo> cultureInfoProvider = null)
        {
            _cultureInfoProvider = cultureInfoProvider ?? (() => CultureInfo.CurrentCulture);
        }

        public string ToUpper(string value)
        {
            return value?.ToUpperInvariant() ?? throw new ArgumentNullException(nameof(value));
        }

        public string ToLower(string value)
        {
            return value?.ToLowerInvariant() ?? throw new ArgumentNullException(nameof(value));
        }

        public int Len(string value)
        {
            return value?.Length ?? throw new ArgumentNullException(nameof(value));
        }

        public string Substring(string value, int startIndex, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if (startIndex < 0) throw new ArgumentException("Start index must be greater than zero");
            if (length < 1) throw new ArgumentException("Length must be greater than one");

            return value.Substring(startIndex, length);
        }

        public string PadLeft(string value, int totalWidth, char padChar = ' ')
        {
            if (totalWidth < 1) throw new ArgumentException("Total width must be greater than zero");
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.PadLeft(totalWidth, padChar);
        }

        public string PadRight(string value, int totalWidth, char padChar = ' ')
        {
            if (totalWidth < 1) throw new ArgumentException("Total width must be greater than zero");
            if (value == null) throw new ArgumentNullException(nameof(value));
            return value.PadRight(totalWidth, padChar) ?? throw new ArgumentNullException(nameof(value));
        }

        public string ToString(object value)
        {
            return Convert.ToString(value, _cultureInfoProvider.Invoke());
        }
    }
}