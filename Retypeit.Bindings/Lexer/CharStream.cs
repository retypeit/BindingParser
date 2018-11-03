using System;
using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer
{
    public class CharStream
    {
        public const char EofChar = '\0';
        private readonly IList<char> _source;

        public CharStream(ICollection<char> source)
        {
            _source = new List<char>(source);
        }

        public bool Eof => _source.Count <= CurrentIndex;
        public int CurrentIndex { get; set; }

        public char Current => GetChar(CurrentIndex);

        public bool Peek(string expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            for (int i = 0; i < expr.Length; i++)
            {
                if (GetChar(CurrentIndex + i) != expr[i])
                    return false;
            }

            return true;
        }

        public char Peek()
        {
            return GetChar(CurrentIndex + 1);
        }

        public char Read()
        {
            if (_source.Count <= CurrentIndex || _source.Count <= ++CurrentIndex)
                return EofChar;

            return _source[CurrentIndex];
        }

        private char GetChar(int index)
        {
            if (_source.Count <= index)
                return EofChar;

            return _source[index];
        }

        public void Skip(char charToSkip)
        {
            if (Current == charToSkip)
                while (!Eof)
                    if (Read() != ' ')
                        break;
        }

        /// <summary>
        ///     Try to match the provided value, if it succeeds it will advance the stream to the end of the value
        /// </summary>
        /// <param name="valueToMatch">Match a string  value</param>
        /// <param name="ignoreCase">True to ignore character casing</param>
        /// <returns></returns>
        public bool TryMatch(string valueToMatch, bool ignoreCase)
        {
            if (valueToMatch == null)
                throw new ArgumentNullException(nameof(valueToMatch));

            if (CurrentIndex + valueToMatch.Length > _source.Count)
                return false;

            for (var i = 0; i < valueToMatch.Length; i++)
            {
                var charToMatch = valueToMatch[i];
                var sourceChar = _source[CurrentIndex + i];
                if (sourceChar != charToMatch)
                {
                    if (ignoreCase)
                    {
                        if (char.ToUpperInvariant(sourceChar) != char.ToUpperInvariant(charToMatch)) return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            CurrentIndex += valueToMatch.Length;
            return true;
        }
    }
}