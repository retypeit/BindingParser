using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    /// <summary>
    ///     Base class used by token resolvers
    /// </summary>
    public abstract class TokenResolverBase : ITokenResolver
    {
        public bool TryResolve(CharStream stream, ICollection<Token> tokens)
        {
            var posBeforeMatch = stream.CurrentIndex;

            if (DoTryResolve(stream, tokens)) return true;
            stream.CurrentIndex = posBeforeMatch;
            return false;

        }

        /// <summary>
        ///     Tries to resolve a specific token, if it fails (throws an exception or returns false)
        ///     any progress on the stream will be reset.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        protected abstract bool DoTryResolve(CharStream stream, ICollection<Token> tokens);
    }
}