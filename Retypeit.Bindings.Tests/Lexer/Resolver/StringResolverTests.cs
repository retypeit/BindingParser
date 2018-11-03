using System.Collections.Generic;
using System.Linq;
using Retypeit.Scripts.Bindings.Lexer;
using Retypeit.Scripts.Bindings.Lexer.Resolvers;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Lexer.Resolver
{
    public class StringResolverTests
    {
        [Fact]
        public void Resolve_StringFollowedBySpace_ShouldMoveStreamToSpace()
        {
            // Setup test
            var stream = new CharStream("\"str\" a".ToCharArray());
            var resolver = new StringResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            var token = tokens.First();
            Assert.NotNull(token);
            Assert.Equal(TokenTypes.String, token.Type);
            Assert.Equal("str", token.Value);
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_StringContainingDoubleQuotes_ShouldConvertDobuleQuotesIntoQuote()
        {
            // Setup test
            var stream = new CharStream("\"str\"\"\" a".ToCharArray());
            var resolver = new StringResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            var token = tokens.First();
            Assert.True(result);
            Assert.NotNull(token);
            Assert.Equal(TokenTypes.String, token.Type);
            Assert.Equal("str\"", token.Value);
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_StringContainingDoubleQuotes_ShouldConvertDobuleQuotesIntoQuote2()
        {
            // Setup test
            var stream = new CharStream("\"st\"\"r\" ".ToCharArray());
            var resolver = new StringResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);

            var token = tokens.First();
            Assert.NotNull(token);
            Assert.Equal(TokenTypes.String, token.Type);
            Assert.Equal("st\"r", token.Value);
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_StartQuoteFollowedByLetters_ShouldNotFindAStringSinceThereIsNoEndQuote()
        {
            // Setup test
            var stream = new CharStream("\"str".ToCharArray());
            var resolver = new StringResolver();
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