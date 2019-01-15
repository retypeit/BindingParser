using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    /// <summary>
    ///     Resolves identities such as: function or variable names
    /// </summary>
    public class IdentityResolver : TokenResolverBase
    {
        // List of characters that (besides letters) are valid in an identity
        public char[] ValidSpecialCharacters = {'.', '_'};
        public char[] ValidSpecialStartingCharacters = {'@'};

        protected override bool DoTryResolve(CharStream stream, ICollection<Token> tokens)
        {
            if (stream.Eof)
                return false;

            // Validate that the first character is either a letter or @
            if (!char.IsLetter(stream.Current) && !ValidSpecialStartingCharacters.Contains(stream.Current))
                return false;

            var identityBuilder = new StringBuilder();
            // Add first character
            identityBuilder.Append(stream.Current);
            stream.Read();

            // Add the rest of the identity
            while (!stream.Eof)
            {
                var c = stream.Current;
                if (char.IsLetter(c) || char.IsNumber(c) || ValidSpecialCharacters.Contains(c))
                {
                    identityBuilder.Append(c);
                }
                else
                {
                    tokens.Add(new Token(TokenTypes.Identity, identityBuilder.ToString()));
                    return true;
                }

                stream.Read();
            }

            if (identityBuilder.Length > 0)
            {
                tokens.Add(new Token(TokenTypes.Identity, identityBuilder.ToString()));
                return true;
            }

            return false;
        }
    }
}