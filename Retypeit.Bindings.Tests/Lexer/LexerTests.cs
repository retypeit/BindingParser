using System.Linq;
using Retypeit.Scripts.Bindings.Lexer;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Lexer
{
    public class LexerTests
    {
        [Fact]
        public void Scan_ShouldWorkWithVariable()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{variable}").ToList();
            Assert.Equal(TokenTypes.LeftBracket, result[0].Type);
            Assert.Equal(TokenTypes.Identity, result[1].Type);
            Assert.Equal(TokenTypes.RightBracket, result[2].Type);
        }

        [Fact]
        public void Scan_ShouldWorkTextBlock()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("abcdef").ToList();
            Assert.Equal(TokenTypes.TextBlock, result[0].Type);
            Assert.Equal("abcdef", result[0].Value);
        }

        //"len(\"str\""

        [Fact]
        public void Scan_ShouldWorkWithString()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{ variable \"hej\"}").ToList();
            Assert.Equal(TokenTypes.LeftBracket, result[0].Type);
            Assert.Equal(TokenTypes.Identity, result[1].Type);
            Assert.Equal(TokenTypes.String, result[2].Type);
            Assert.Equal(TokenTypes.RightBracket, result[3].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithChar()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{'a'}").ToList();
            var index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Char, result[index].Type);
            Assert.Equal('a', result[index++].Value);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithInteger()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{1}").ToList();
            var index = 0;

            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index].Type);
            Assert.Equal(1, result[index++].Value);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithDecimal()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{1.1}").ToList();
            var index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Decimal, result[index].Type);
            Assert.Equal(new decimal(1.1), result[index++].Value);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithFunctionThatHasVariablesAsArguments()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{variable(p1, p2)}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.LeftParentheses, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.Comma, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.RightParentheses, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithFunctionWithParameter()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{len(\"str\")}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.LeftParentheses, result[index++].Type);
            Assert.Equal(TokenTypes.String, result[index++].Type);
            Assert.Equal(TokenTypes.RightParentheses, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithDecimalPlusString()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{1.5 + \"kg\"}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Decimal, result[index].Type);
            Assert.Equal(new decimal(1.5), result[index++].Value);
            Assert.Equal(TokenTypes.Plus, result[index++].Type);
            Assert.Equal(TokenTypes.String, result[index].Type);
            Assert.Equal(TokenTypes.String, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWhenAddingToNumericValues()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{ 1+2000}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index].Type);
            Assert.Equal(1, result[index++].Value);
            Assert.Equal(TokenTypes.Plus, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index].Type);
            Assert.Equal(2000, result[index++].Value);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWhenSubstractingNumericValues()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{12-1.1}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index].Type);
            Assert.Equal(12, result[index++].Value);
            Assert.Equal(TokenTypes.Minus, result[index++].Type);
            Assert.Equal(TokenTypes.Decimal, result[index].Type);
            Assert.Equal(new decimal(1.1), result[index++].Value);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_ShouldWorkWithIfElseStatements()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("{1 ? 2 : 3 + 2}").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index++].Type);
            Assert.Equal(TokenTypes.QuestionMark, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index++].Type);
            Assert.Equal(TokenTypes.Colon, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index++].Type);
            Assert.Equal(TokenTypes.Plus, result[index++].Type);
            Assert.Equal(TokenTypes.Integer, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index].Type);
        }

        [Fact]
        public void Scan_TextBlocksAndBindingsShouldWork()
        {
            var lexer = new BindingLexer();
            var result = lexer.Scan("hello {name} the temperature is {temp}degrees").ToList();
            int index = 0;
            Assert.Equal(TokenTypes.TextBlock, result[index++].Type);
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index++].Type);
            Assert.Equal(TokenTypes.TextBlock, result[index++].Type);
            Assert.Equal(TokenTypes.LeftBracket, result[index++].Type);
            Assert.Equal(TokenTypes.Identity, result[index++].Type);
            Assert.Equal(TokenTypes.RightBracket, result[index++].Type);
            Assert.Equal(TokenTypes.TextBlock, result[index].Type);
        }

        [Fact]
        public void Scan_NestedBindingBlocksShouldNotWork()
        {
            var lexer = new BindingLexer();
            Assert.Throws<LexerException>(() => lexer.Scan("{1+{2}}"));
        }

        [Fact]
        public void Scan_JavaScriptStyleBlock_ShouldWork()
        {
            var lexer = new BindingLexer(BlockStyles.JavaScript);
            var tokens = lexer.Scan("Testing JS blocks: !{1}").ToList();
            Assert.Equal(5, tokens.Count);
        }


        [Fact]
        public void Scan_CShartStyleLexerWithJavaScriptData_ShouldFailToFindScriptBlock()
        {
            var lexer = new BindingLexer(BlockStyles.JavaScript);
            var tokens = lexer.Scan("Testing JS blocks: {1}").ToList();
            Assert.Equal(2, tokens.Count);
        }
    }
}