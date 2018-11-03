using System;
using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    public class KeywordResolver : TokenResolverBase
    {
        /// <summary>
        ///     Resolves the given keyword and translates it into the token type
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        public KeywordResolver(string keyword, TokenTypes type)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("message", nameof(keyword));

            Type = type;
            Keyword = keyword;
        }

        public TokenTypes Type { get; }
        public string Keyword { get; }

        protected override bool DoTryResolve(CharStream stream, HashSet<Token> tokens)
        {
            if (!stream.TryMatch(Keyword, true))
                return false;

            tokens.Add(new Token(Type));
            return true;
        }
    }
}