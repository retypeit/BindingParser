using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    public class CharResolver : TokenResolverBase
    {
        protected override bool DoTryResolve(CharStream stream, HashSet<Token> tokens)
        {
            if (stream.Eof)
                return false;

            // Must start with a letter
            if (stream.Current != '\'') return false;

            var charValue = '\0';
            while (!stream.Eof)
            {
                var c = stream.Read();
                if (c == '\'')
                {
                    if (stream.TryMatch("\'\'", false))
                    {
                        tokens.Add(new Token(TokenTypes.Char, '\''));
                        return true;
                    }

                    stream.Read();
                    tokens.Add(new Token(TokenTypes.Char, charValue));
                    return true;
                }

                charValue = c;
            }

            return false;
        }
    }
}