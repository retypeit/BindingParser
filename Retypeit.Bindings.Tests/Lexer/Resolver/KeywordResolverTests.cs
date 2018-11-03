using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Lexer;
using Retypeit.Scripts.Bindings.Lexer.Resolvers;
using Retypeit.Scripts.Bindings.Tests.Helpers;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Lexer.Resolver
{
    public class KeywordResolverTests
    {
        [Fact]
        public void Resolve_KeywordFollowedByLetter_ShouldMoveStreamToLetter()
        {
            // Setup test
            var stream = new CharStream("(a".ToCharArray());
            var resolver = new KeywordResolver("(", TokenTypes.LeftParentheses);
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            AssertHelper.Equal(tokens, new Token(TokenTypes.LeftParentheses));
            Assert.Equal('a', stream.Current);
        }
    }
}