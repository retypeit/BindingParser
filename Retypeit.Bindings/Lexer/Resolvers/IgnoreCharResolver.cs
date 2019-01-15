using System.Collections.Generic;
using System.Linq;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    public class IgnoreCharResolver : TokenResolverBase
    {
        private readonly char[] _charsToIgnore;

        /// <summary>
        ///     Will skip the provided characters
        /// </summary>
        public IgnoreCharResolver(params char[] charsToIgnore)
        {
            _charsToIgnore = charsToIgnore;
        }

        protected override bool DoTryResolve(CharStream stream, ICollection<Token> tokens)
        {
            var result = false;
            while (!stream.Eof)
            {
                if (!_charsToIgnore.Contains(stream.Current))
                    return result;

                result = true;
                stream.Read();
            }

            return result;
        }
    }
}