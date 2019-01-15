using System.Linq;
using Retypeit.Scripts.Bindings.Parser;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Exceptions;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Lexer;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Parser
{
    public class ParserTests
    {
        private IAstNode Parse(string script, params FunctionInfo[] validFunctions)
        {
            var lexer = new BindingLexer();
            var tokens = lexer.Scan(script);
            var parser = new BindingParser();
            return parser.Parse(tokens, validFunctions)?.Child;
        }

        [Fact]
        public void Parse_EmptyString_ShouldWork()
        {
            var result = Parse($"");
        }

        [Fact]
        public void Parse_FunctionWithInvalidNumberOfParameters_ShouldThrowException()
        {
            var methodName = "methodname";
            var exception = Assert.Throws<ParserException>(() =>
                Parse($"{{{methodName}(\"str\")}}", new FunctionInfo(methodName, typeof(string), typeof(int))));
            Assert.Contains($"{methodName}(String,Int32)", exception.Message);
        }

        [Fact]
        public void Parse_IfElseStatement_ShouldChooseElseBlock()
        {
            // Perform test
            var programNode = Parse("{1==1 ? 2 : 3 + 2}") as IfElseNode;

            // Validate result

            // Validate root node type
            Assert.NotNull(programNode);

            // Validate condition type
            var conditionNode = programNode.Condition as EqualNode;
            Assert.NotNull(conditionNode);

            Assert.IsType<ValueNode>(conditionNode.Value1);
            Assert.IsType<ValueNode>(conditionNode.Value2);

            Assert.IsType<ValueNode>(programNode.IfInstr);
            Assert.IsType<AddNode>(programNode.ElseInstr);

            // Validate else instructions nodetype
            var addNode = programNode.ElseInstr as AddNode;
            Assert.NotNull(addNode);

            Assert.IsType<ValueNode>(addNode.Value1);
            Assert.IsType<ValueNode>(addNode.Value2);
        }

        [Fact]
        public void Parse_LenShouldReturnStringLength()
        {
            var program = Parse("{len(\"str\")}", new FunctionInfo("len", typeof(string)));

            Assert.IsType<FunctionNode>(program);

            var func = (FunctionNode) program;

            Assert.Single(func.Parameters);
            Assert.IsType<ValueNode>(func.Parameters.First());
        }

        [Fact]
        public void Parse_ShouldFailWhenDivideIsFirst()
        {
            Assert.Throws<ParserException>(() => Parse("{/1}"));
        }

        [Fact]
        public void Parse_ShouldFailWhenDivideIsLast()
        {
            Assert.Throws<ParserException>(() => Parse("{1/ }"));
        }

        [Fact]
        public void Parse_UnknownFunction_ShouldThrowException()
        {
            var exception =
                Assert.Throws<ParserException>(() => Parse("{trim(\"str\")}", new FunctionInfo("len", typeof(string))));
            Assert.Contains("trim", exception.Message);
        }
    }
}