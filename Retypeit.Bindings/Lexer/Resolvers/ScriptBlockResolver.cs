using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    public class ScriptBlockResolver : TokenResolverBase
    {
        private readonly BlockStyles _style;
        private readonly ITokenResolver[] _resolvers;

        private readonly char[] _terminatorChars =
            {',', '!', '=', '.', '*', '-', '+', '?', ')', '(', '{', '}', ' ', '/', ':'};

        /// <summary>
        ///     Resolves a script block
        /// </summary>
        /// s
        public ScriptBlockResolver(BlockStyles style)
        {
            _style = style;
            _resolvers = new ITokenResolver[]
            {
                new KeywordResolver("true", TokenTypes.True),
                new KeywordResolver("false", TokenTypes.False),
                new KeywordResolver("null", TokenTypes.Null),
                new KeywordResolver("???", TokenTypes.DefaultUndefinedOrNullValue),
                new KeywordResolver("??", TokenTypes.DefaultNullValue),
                new KeywordResolver(">", TokenTypes.GreaterThan),
                new KeywordResolver("<", TokenTypes.LessThan),
                new KeywordResolver("!=", TokenTypes.NotEqual),
                new KeywordResolver("==", TokenTypes.Equal),
                new KeywordResolver("+", TokenTypes.Plus),
                new KeywordResolver("-", TokenTypes.Minus),
                new KeywordResolver("*", TokenTypes.Times),
                new KeywordResolver("/", TokenTypes.Divide),
                new KeywordResolver(":", TokenTypes.Colon),
                new KeywordResolver("?", TokenTypes.QuestionMark),
                new KeywordResolver("(", TokenTypes.LeftParentheses),
                new KeywordResolver(")", TokenTypes.RightParentheses),
                new KeywordResolver(",", TokenTypes.Comma),
                new NumberResolver(_terminatorChars),
                new IdentityResolver(),
                new StringResolver(),
                new CharResolver(),
                new IgnoreCharResolver(' ', '\n', '\t')
            };
        }

        protected override bool DoTryResolve(CharStream stream, HashSet<Token> tokens)
        {
            if (stream.Eof)
                return false;

            // Must start with a letter
            if (_style == BlockStyles.CSharp)
            {
                if (!stream.TryMatch("{", false))
                    return false;
            }
            else if (_style == BlockStyles.JavaScript)
            {
                if (!stream.TryMatch("!{", false))
                    return false;
            }


            tokens.Add(new Token(TokenTypes.LeftBracket));

            while (!stream.Eof)
            {
                if (stream.Current == '}')
                {
                    stream.Read();
                    tokens.Add(new Token(TokenTypes.RightBracket));
                    return true;
                }

                var tokenFound = false;
                foreach (var resolver in _resolvers)
                {
                    tokenFound = resolver.TryResolve(stream, tokens);
                    if (tokenFound)
                        break;
                }

                if (!tokenFound)
                    throw new LexerException($"Syntax error! Unexpected char: [{stream.Current}]", tokens);
            }

            tokens.Add(new Token(TokenTypes.Eof));
            return false;
        }
    }
}