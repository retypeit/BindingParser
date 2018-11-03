using System.Collections.Generic;
using System.Linq;
using Retypeit.Scripts.Bindings.Lexer;
using Retypeit.Scripts.Bindings.Lexer.Resolvers;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Lexer.Resolver
{
    public class NumberResolverTests
    {
        [Fact]
        public void Resolve_IntegerFollowedBySpace_ShouldMoveStreamToSpace()
        {
            // Setup test
            var stream = new CharStream("12 a".ToCharArray());
            var resolver = new NumberResolver(' ');
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            var token = tokens.First();

            Assert.Equal(TokenTypes.Integer, token.Type);
            Assert.Equal(12, token.Value);
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_IntegerValue_ShouldMoveStreamToEoF()
        {
            // Setup test
            var stream = new CharStream("12".ToCharArray());
            var resolver = new NumberResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            var token = tokens.First();
            Assert.Equal(TokenTypes.Integer, token.Type);
            Assert.Equal(12, token.Value);
            Assert.Equal(CharStream.EofChar, stream.Current);
        }

        [Fact]
        public void Resolve_DecimalValueFollowedBySpace_ShouldMoveStreamToSpace()
        {
            // Setup test
            var stream = new CharStream("1.2 ".ToCharArray());
            var resolver = new NumberResolver(' ');
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            var token = tokens.First();
            Assert.True(result);
            Assert.Equal(TokenTypes.Decimal, token.Type);
            Assert.Equal(new decimal(1.2), token.Value);
            Assert.Equal(' ', stream.Current);
        }

        [Fact]
        public void Resolve_DecimalValue_ShouldMoveStreamToEof()
        {
            // Setup test
            var stream = new CharStream("1.2".ToCharArray());
            var resolver = new NumberResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.True(result);
            var token = tokens.First();
            Assert.Equal(TokenTypes.Decimal, token.Type);
            Assert.Equal(new decimal(1.2), token.Value);
            Assert.Equal(CharStream.EofChar, stream.Current);
        }

        /// <summary>
        /// Decimal value with comma as separator should fail since comma is not a valid separator char
        /// in the script
        /// </summary>
        [Fact]
        public void Resolve_DecimalValueWithCommaAsSeparator_ShouldFail()
        {
            // Setup test
            var stream = new CharStream("1,2".ToCharArray());
            var resolver = new NumberResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.False(result);
        }

        /// <summary>
        /// Integer followed by a letter should fail since letter is not a valid terminator char
        /// </summary>
        [Fact]
        public void Resolve_IntegerFollowedByLetter_ShouldFail()
        {
            // Setup test
            var stream = new CharStream("12a".ToCharArray());
            var resolver = new NumberResolver();
            var tokens = new HashSet<Token>();

            // Perform test
            var result = resolver.TryResolve(stream, tokens);

            // Validate result
            Assert.False(result);
            Assert.Equal(0, stream.CurrentIndex);
        }
    }
}