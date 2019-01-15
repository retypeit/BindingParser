using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    public class NumberResolver : TokenResolverBase
    {
        private readonly char[] _terminatorChars;

        /// <summary>
        ///     Resolves a numeric value
        /// </summary>
        /// <param name="terminatorChars">Chars that will terminate the search for a numeric value</param>
        public NumberResolver(params char[] terminatorChars)
        {
            _terminatorChars = terminatorChars;
        }

        protected override bool DoTryResolve(CharStream stream, ICollection<Token> tokens)
        {
            if (stream.Eof)
                return false;

            // Must start with a letter
            if (!char.IsNumber(stream.Current)) return false;

            var valueBuffer = new StringBuilder();
            var isDecimal = false;

            var c = stream.Current;

            while (!stream.Eof)
            {
                if (!char.IsNumber(c) && c != '.')
                {
                    break;
                }

                if (c == '.')
                {
                    if (!char.IsNumber(stream.Peek())) // Is next char numeric?
                        return false;
                    isDecimal = true;
                }

                // Valid number of decimal.. cache it
                valueBuffer.Append(c);
                c = stream.Read();
            }

            // We do not have any buffered value
            if (valueBuffer.Length == 0)
                return false;

            // Terminated by a valid char?
            if (!_terminatorChars.Contains(c) && !stream.Eof)
                return false;

            tokens.Add(CreateToken(valueBuffer, isDecimal));
            return true;
        }

        private static Token CreateToken(StringBuilder value, bool isDecimal)
        {
            if (isDecimal)
            {
                if (!decimal.TryParse(value.ToString(), NumberStyles.Any,
                    new NumberFormatInfo {NumberDecimalSeparator = "."}, out var parsedValue))
                    return null; // Error parsing the value.. we could not handle the token

                return new Token(TokenTypes.Decimal, parsedValue);
            }
            else
            {
                if (!int.TryParse(value.ToString(), out var parsedValue))
                    return null; // Error parsing the value.. we could not handle the token

                return new Token(TokenTypes.Integer, parsedValue);
            }
        }
    }
}