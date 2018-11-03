using System.Collections.Generic;
using System.Text;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    /// <summary>
    ///     Resolves a string value
    /// </summary>
    public class StringResolver : TokenResolverBase
    {
        protected override bool DoTryResolve(CharStream stream, HashSet<Token> tokens)
        {
            if (stream.Eof)
                return false;

            // Must start with a letter
            if (!stream.TryMatch("\"", false)) return false;

            var stringValue = new StringBuilder();
            while (!stream.Eof)
            {
                var c = stream.Current;
                if (c == '"')
                {
                    if (stream.TryMatch("\"\"", false))
                    {
                        stringValue.Append("\"");
                        continue;
                    }

                    stream.Read();

                    tokens.Add(new Token(TokenTypes.String, stringValue.ToString()));
                    return true;
                }

                stringValue.Append(c);
                stream.Read();
            }

            return false;
        }
    }
}