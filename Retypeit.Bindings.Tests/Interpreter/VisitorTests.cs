using System;
using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Interpreter;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests.Interpreter
{
    public class VisitorTests
    {
        [Fact]
        public void Visit_AddDoubleValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((double) 4);
            var rightValue = new ValueNode(1);
            Assert.Equal((decimal) 5, visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddFloatValueToDoubleValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 4.2);
            var rightValue = new ValueNode(1.2);
            Assert.Equal((decimal) 5.4, visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddFloatValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 4.2);
            var rightValue = new ValueNode(1);
            Assert.Equal((decimal) 5.2, visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddIntValueToIntValue_ShouldReturnTheResult()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(2);
            Assert.Equal(6, visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddIntValueToNullValue_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(null);
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddIntValueToObject_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(new object());
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddIntValueToStringValue_ShouldReturnAStringWithBothValues()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode("1");
            Assert.Equal("41", visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddIntValueToStringValue_ShouldReturnTheResult()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode("kg");
            Assert.Equal("4kg", visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_AddStringValueToNullValue_ShouldReturnResult()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode("i=");
            var rightValue = new ValueNode(null);
            Assert.Equal("i=", visitor.Visit(new AddNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideDoubleValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((double) 4);
            var rightValue = new ValueNode(4);
            Assert.Equal((decimal) 1, visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideFloatValueToDoubleValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode((double) 2);
            Assert.Equal((decimal) 2.5, visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideFloatValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 4);
            var rightValue = new ValueNode(2);
            Assert.Equal((decimal) 2, visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideFloatValueToStringValue_ShouldThrowExceptiopn()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode("2");
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideIntValueToIntValue_ShouldReturnTheResult()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(2);
            Assert.Equal(2, visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideIntValueToStringValue_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode("kg");
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideValueByNull_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(null);
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_DivideValueByZero_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(0);
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new DivideNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_EqualDoubleToDecimal_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((double) 5);
            var rightValue = new ValueNode((decimal) 5);
            Assert.True((bool) visitor.Visit(new EqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_EqualFloatToBool_ShouldReturnFalse()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode(true);

            Assert.False((bool) visitor.Visit(new EqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_EqualFloatToDouble_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode((double) 5);
            Assert.True((bool) visitor.Visit(new EqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_EqualIntToDecimal_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(5);
            var rightValue = new ValueNode((decimal) 5);
            Assert.True((bool) visitor.Visit(new EqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_EqualIntToInt_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(5);
            var rightValue = new ValueNode(5);
            Assert.True((bool) visitor.Visit(new EqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_FunctionThatReturnsInt_ShouldReturnExpectedValue()
        {
            var visitor = new ScriptVisitor(context => 1);
            Assert.Equal(1, visitor.Visit(new FunctionNode("function name", new List<IAstNode>())));
        }

        [Fact]
        public void Visit_FunctionThatThrowsException_ExceptionShouldBeThrownOutOfTheVisitor()
        {
            var visitor = new ScriptVisitor(context => throw new InvalidOperationException("some error"));
            Assert.Throws<InvalidOperationException>(() =>
                visitor.Visit(new FunctionNode("function name", new List<IAstNode>())));
        }

        [Fact]
        public void Visit_FunctionWithSpecificName_NameShouldBePassedToInvoker()
        {
            var functionName = "NameOfFunction";
            var visitor = new ScriptVisitor(context =>
            {
                Assert.Equal(functionName, context.Name);
                return true;
            });
            visitor.Visit(new FunctionNode(functionName, new List<IAstNode>()));
        }

        [Fact]
        public void Visit_IfElseThrowsException_ExceptionShouldBeThrownOutsideTheVisitor()
        {
            var visitor = new ScriptVisitor(context => throw new InvalidOperationException("Operation failed"));
            var conditionInstr = new ValueNode(true);
            var ifInstr = new FunctionNode("name", new List<IAstNode>());
            var elseInstr = new ValueNode(false);
            Assert.Throws<InvalidOperationException>(() =>
                visitor.Visit(new IfElseNode(conditionInstr, ifInstr, elseInstr)));
        }

        [Fact]
        public void Visit_IfElseTrueElsePart_ShouldRunElseInstructions()
        {
            var resultValue = Guid.NewGuid().ToString();
            var visitor = new ScriptVisitor(context => 1);
            var conditionInstr = new ValueNode(false);
            var ifInstr = new ValueNode(true);
            var elseInstr = new ValueNode(resultValue);
            Assert.Equal(resultValue, visitor.Visit(new IfElseNode(conditionInstr, ifInstr, elseInstr)));
        }

        [Fact]
        public void Visit_IfElseTrueIfPart_ShouldRunIfInstructions()
        {
            var resultValue = Guid.NewGuid().ToString();
            var visitor = new ScriptVisitor(context => 1);
            var conditionInstr = new ValueNode(true);
            var ifInstr = new ValueNode(resultValue);
            var elseInstr = new ValueNode(false);
            Assert.Equal(resultValue, visitor.Visit(new IfElseNode(conditionInstr, ifInstr, elseInstr)));
        }

        [Fact]
        public void Visit_MultiplyFloatAndDecimal_ShouldRunIfInstructions()
        {
            var visitor = new ScriptVisitor(context => 1);
            var value1 = new ValueNode((float) 5);
            var value2 = new ValueNode((decimal) 3.2);
            Assert.Equal((decimal) 16, visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_MultiplyFloatAndFunctionThatThrowsException_ExceptionShouldBeThrownOutsideTheVisitor()
        {
            var visitor = new ScriptVisitor(context => throw new InvalidOperationException("Operation failed"));
            var value1 = new ValueNode((float) 5);
            var value2 = new FunctionNode("name", new List<IAstNode>());
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_MultiplyFloatAndNull_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 0);
            var value1 = new ValueNode((float) 5);
            var value2 = new ValueNode(null);
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_MultiplyFloatAndObject_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 0);
            var value1 = new ValueNode((float) 5);
            var value2 = new ValueNode(new object());
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_MultiplyIntAndDecimal_ShouldRunIfInstructions()
        {
            var visitor = new ScriptVisitor(context => 1);
            var value1 = new ValueNode(2);
            var value2 = new ValueNode((decimal) 3.2);
            Assert.Equal((decimal) 6.4, visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_MultiplyIntAndInt_ShouldRunIfInstructions()
        {
            var visitor = new ScriptVisitor(context => 1);
            var value1 = new ValueNode(2);
            var value2 = new ValueNode(3);
            Assert.Equal(6, visitor.Visit(new MultiplyNode(value1, value2)));
        }

        [Fact]
        public void Visit_NotEqualDoubleToDecimal_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((double) 5);
            var rightValue = new ValueNode((decimal) 2);
            Assert.True((bool) visitor.Visit(new NotEqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_NotEqualFloatToBool_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode(true);
            Assert.True((bool) visitor.Visit(new NotEqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_NotEqualFloatToDouble_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 5);
            var rightValue = new ValueNode((double) 2);
            Assert.True((bool) visitor.Visit(new NotEqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_NotEqualIntToDecimal_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(5);
            var rightValue = new ValueNode((decimal) 5);
            Assert.False((bool) visitor.Visit(new NotEqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_NotEqualIntToInt_ShouldReturnTrue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(5);
            var rightValue = new ValueNode(5);
            Assert.False((bool) visitor.Visit(new NotEqualNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubDoubleValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((double) 4);
            var rightValue = new ValueNode(1);
            Assert.Equal((decimal) 3, visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubFloatValueToDoubleValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 4.2);
            var rightValue = new ValueNode(1.2);
            Assert.Equal((decimal) 3, visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubFloatValueToIntValue_ShouldReturnTheResultAsDecimal()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode((float) 4.2);
            var rightValue = new ValueNode(1);
            Assert.Equal((decimal) 3.2, visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubIntValueToIntValue_ShouldReturnTheResult()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(2);
            Assert.Equal(2, visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubIntValueToNullValue_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(null);
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubIntValueToObject_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode(new object());
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_SubIntValueToStringValue_ShouldThrowException()
        {
            var visitor = new ScriptVisitor(context => 1);
            var leftValue = new ValueNode(4);
            var rightValue = new ValueNode("1");
            Assert.Throws<InvalidOperationException>(() => visitor.Visit(new SubNode(leftValue, rightValue)));
        }

        [Fact]
        public void Visit_ValueNode_ShouldReturnIntegerValue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var resultingValue = 6;
            Assert.Equal(resultingValue, visitor.Visit(new ValueNode(resultingValue)));
        }

        [Fact]
        public void Visit_ValueNode_ShouldReturnStringValue()
        {
            var visitor = new ScriptVisitor(context => 1);
            var resultingValue = "string value";
            Assert.Equal(resultingValue, visitor.Visit(new ValueNode(resultingValue)));
        }
    }
}