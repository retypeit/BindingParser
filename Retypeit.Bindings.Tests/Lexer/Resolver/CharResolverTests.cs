using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Lexer;
using Retypeit.Scripts.Bindings.Lexer.Resolvers;
using Retypeit.Scripts.Bindings.Tests.Helpers;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Lexer.Resolver
{
    public class CharResolverTests
    {
        [Fact]
        public void Resolve_CharAndSpace_ShouldMoveToSpace()
        {
            // Setup test
            var stream = new CharStream("'a' ".ToCharArray());
            var resolver = new CharResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            AssertHelper.Equal(tokens, new Token(TokenTypes.Char, 'a'));
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_TrippleQuotes_ShouldBeInterpretedAsSingleQuotationMarkOfTypeChar()
        {
            // Setup test
            var stream = new CharStream("''' ".ToCharArray());
            var resolver = new CharResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            AssertHelper.Equal(tokens, new Token(TokenTypes.Char, '\''));
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_ScriptWithStartOfCharButNoEndOfChar_ShouldNotFindAChar()
        {
            // Setup test
            var stream = new CharStream("{'str}".ToCharArray());
            var resolver = new CharResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.False(result);
            Assert.Empty(tokens);
            Assert.Equal(0, stream.CurrentIndex);
        }
    }
}