using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Lexer.Resolvers;

namespace Retypeit.Scripts.Bindings.Lexer
{
    public class BindingLexer
    {
        private readonly ITokenResolver[] _resolvers;

        public BindingLexer(BlockStyles style = BlockStyles.CSharp)
        {
            _resolvers = new ITokenResolver[]
            {
                new ScriptBlockResolver(style),
                new TextBlockResolver(style)
            };
        }

        public ICollection<Token> Scan(string expression)
        {
            var tokens = new HashSet<Token>();
            var stream = new CharStream(expression.ToCharArray());
            while (!stream.Eof)
            {
                var tokenFound = false;
                foreach (var resolver in _resolvers)
                {
                    tokenFound = resolver.TryResolve(stream, tokens);
                    if (tokenFound)
                        break;
                }

                if (!tokenFound)
                    throw new LexerException($"Unexpected char: {stream.Current}", tokens);
            }

            tokens.Add(new Token(TokenTypes.Eof));
            return tokens;
        }
    }
}