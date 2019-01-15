using System;
using System.Collections.Generic;
using System.Globalization;
using Retypeit.Scripts.Bindings;
using Retypeit.Scripts.Bindings.Exceptions;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Tests.Mock;
using Xunit;

namespace Retypeit.Scripts.Bindings.Tests
{
    public class BindingResolverTests
    {
        private object Run(string script, Dictionary<string, object> variables = null)
        {
            var resolver = new BindingResolver {CultureInfo = CultureInfo.InvariantCulture};

            if(variables == null)
                return resolver.Resolve(script);

            return resolver.Resolve(script, variables);
        }

        [Fact]
        public void Resolve_AddBoolAndString_ShouldReturnStringThatCombinesBothValues()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"value2", true}, {"value1", "Value = "}};
            // Run test
            var result = Run("{value1 + value2}", variables);
            // Evaluate result
            Assert.Equal("Value = True", result);
        }

        [Fact]
        public void Resolve_EmptyString_ShouldWork()
        {
            // Run test
            var result = Run("");
            // Evaluate result
            Assert.Equal("", result);
        }

        [Fact]
        public void Resolve_AddDateTimeAndString_ShouldReturnStringThatCombinesBothValues()
        {
            // Setup test
            var variables = new Dictionary<string, object>
                {{"value2", new DateTime(2018, 8, 19, 21, 18, 8)}, {"value1", "Value = "}};
            // Run test
            var result = Run("{value1 + value2}", variables);
            // Evaluate result
            Assert.Equal("Value = 08/19/2018 21:18:08", result);
        }

        [Fact]
        public void Resolve_AddDecimalAndObject_ShouldReturnStringThatCombinesBothValues()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Run("{1.5 +   obj}", new Dictionary<string, object> {{"obj", new object()}}));
        }

        [Fact]
        public void Resolve_AddDoubleAndString_ShouldReturnStringThatCombinesBothValues()
        {
            // Setup test
            var valueToInsert = 1.5;
            var variables = new Dictionary<string, object> {{"number", valueToInsert}};
            // Run test
            var result = Run("{number +   \"kg\"}", variables);
            // Evaluate result
            Assert.Equal("1.5kg", result);
        }

        [Fact]
        public void Resolve_AddIntAndString_ShouldReturnAStringCombiningTheValues()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"number", 1}};
            // Run test
            var result = Run("{number +   \"kg\"}", variables);
            // Evaluate result
            Assert.Equal("1kg", result);
        }

        [Fact]
        public void Resolve_AddNullAndString_ShouldReturnAStringThatIgnoresTheNullValue()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"value2", null}, {"value1", "Value = "}};
            // Run test
            var result = Run("{value1 + value2}", variables);
            // Evaluate result
            Assert.Equal("Value = ", result);
        }

        [Fact]
        public void Resolve_AddStringAndVariable_ShouldReturnStringThatCombinesBothValues()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"number", "5"}};
            // Run test
            var result = Run("{number +   \"kg\"}", variables);
            // Evaluate result
            Assert.Equal("5kg", result);
        }

        [Fact]
        public void Resolve_CombiningTextAndBindingBlocksShouldWork()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"name", "Packman"}, {"temp", 20.5}};
            // Run test
            var result = Run("hello {name} the temperature is {temp}degrees", variables);
            // Evaluate result
            Assert.Equal("hello Packman the temperature is 20.5degrees", result);
        }

        [Fact]
        public void Resolve_ConditionWithFalse_ShouldReturnFalsePart()
        {
            Assert.Equal(2, Run("{false ? 1: 2}"));
        }

        [Fact]
        public void Resolve_ConditionWithTrue_ShouldReturnTruePart()
        {
            Assert.Equal(1, Run("{true ? 1: 2}"));
        }

        [Fact]
        public void Resolve_CustomMiddleware_ShouldBeAbleToRunCustomMiddlewareFunctionAndStandardFunction()
        {
            // Setup test
            var resolver = new BindingResolver {CultureInfo = CultureInfo.InvariantCulture};
            resolver.UseRunner(new ObjectMethodRunner(new InvokerFunctionsMock()));

            // Run test
            var result = resolver.Resolve("X={one()+ToString(5)}");

            // Evaluate result
            Assert.Equal("X=15", result);
        }

        [Fact]
        public void Resolve_Decimal_ShouldProduceDecimal()
        {
            Assert.Equal(new decimal(2.1), Run("{2.1}"));
        }

        [Fact]
        public void Resolve_False_ShouldReturnBooleanFalse()
        {
            Assert.False((bool) Run("{false}"));
        }

        [Fact]
        public void Resolve_IfElseStatement_ShouldChooseElseBlock()
        {
            Assert.Equal(5, Run("{1 == 2 ? 2 : 3+2}"));
        }

        [Fact]
        public void Resolve_IfElseStatement_ShouldChooseIfBlock()
        {
            Assert.Equal(2, Run("{1 == 1 ? 2 : 3}"));
        }

        [Fact]
        public void Resolve_IntDividedByInt_ShouldFloorDivdedValueToClosestInt()
        {
            Assert.Equal(1, Run("{3 / 2}"));
        }

        [Fact]
        public void Resolve_IntMinusDecimal_ShouldReturnDecimalResult()
        {
            Assert.Equal(new decimal(7.5), Run("{10-2.5 }"));
        }

        [Fact]
        public void Resolve_IntValue_ShouldReturnIntValue()
        {
            Assert.Equal(2, Run("{2}"));
        }

        [Fact]
        public void Resolve_Len_StringValue_ShouldReturnLength()
        {
            Assert.Equal(8, Run("{len(\"strvalue\")}"));
        }

        [Fact]
        public void Resolve_MultipleUndefinedVariablesWithDefaults_ShouldReturnFirstFound()
        {
            var variables = new Dictionary<string, object>();
            variables.Add("v3", "hello");
            variables.Add("v4", 1);
            Assert.Equal("hello", Run("{v1 ??? v2 ??? v3 ??? v4}", variables));
        }

        [Fact]
        public void Resolve_MultipleUndefinedVariablesWithDefaults_ShouldReturnFirstThatIsNotNull()
        {
            var variables = new Dictionary<string, object>();
            variables.Add("v3", null);
            variables.Add("v4", 1);
            Assert.Equal(1, Run("{v1 ??? v2 ??? v3 ??? v4}", variables));
        }

        [Fact]
        public void Resolve_NegativeDecimal_ShouldProduceNegativeDecimal()
        {
            Assert.Equal(new decimal(-2.1), Run("{-2.1}"));
        }

        [Fact]
        public void Resolve_NegativeInt_ShouldProduceNegativeInt()
        {
            Assert.Equal(-2, Run("{-2}"));
        }

        [Fact]
        public void Resolve_NestedIfElseStatements_ShouldChooseElseThenIfBlock()
        {
            Assert.Equal(11, Run("{1 == 2 ? 2 : (1==1 ? 11:12) }"));
        }

        [Fact]
        public void Resolve_NestedIfElseStatementsWithoutParentheses_ShouldChooseElseThenIfBlock2()
        {
            Assert.Throws<ParserException>(() => Run("{1 == 2 ? 2 : 1==1 ? 11:12 }"));
        }

        [Fact]
        public void Resolve_Null_ShouldReturnNull()
        {
            Assert.Null(Run("{null}"));
        }

        [Fact]
        public void Resolve_NullValueWithDefaultExpression_ShouldReturnResultOfExpression()
        {
            var variables = new Dictionary<string, object>();
            variables.Add("v1", null);
            Assert.Equal(2, Run("{v1 ?? (1+1) }", variables));
        }

        [Fact]
        public void Resolve_NullWithNullDefaultValue1_ShouldReturnDefaultValue()
        {
            Assert.Equal(1, Run("{null ?? 1}"));
        }

        [Fact]
        public void Resolve_OneEqualsToOne_ShouldReturnTrue()
        {
            Assert.True((bool) Run("{1==1}"));
        }

        [Fact]
        public void Resolve_OneEqualsToTwo_ShouldReturnFalse()
        {
            Assert.False((bool) Run("{1==2}"));
        }

        [Fact]
        public void Resolve_OnePlusOne_ShouldReturnResult()
        {
            Assert.Equal(2, Run("{1+1}"));
        }

        [Fact]
        public void Resolve_OnePlusThreeDividedByTwoPlusTwo_ShouldReturnResult()
        {
            Assert.Equal(4, Run("{1 + 3 / 2 + 2}"));
        }

        [Fact]
        public void Resolve_OnePlusThreePlusTwoPlusTwoDividedByTwo_ShouldReturnResult()
        {
            Assert.Equal(7, Run("{1 + 3 + 2 + 2/2 }"));
        }

        [Fact]
        public void Resolve_OnePlusThreeTimesTwoPlusTwo_ShouldReturnResult()
        {
            Assert.Equal(9, Run("{1 + 3 * 2 + 2}"));
        }

        [Fact]
        public void Resolve_OnePlusTwoTimesThree_ShouldReturnResult()
        {
            Assert.Equal(7, Run("{1+2*3}"));
        }

        [Fact]
        public void Resolve_OneTimesThreePlusTwo_ShouldReturnResult()
        {
            Assert.Equal(5, Run("{1 * 3 + 2}"));
        }

        [Fact]
        public void Resolve_PadRight_MissingParameter2_ShouldFail()
        {
            Assert.Throws<ParserException>(() => Run("{PadRight(\"str\",)}"));
        }

        [Fact]
        public void Resolve_PadRight_MissingRequiredParameter_ShouldFail()
        {
            Assert.Throws<ParserException>(() => Run("{PadRight(\"str\")}"));
        }

        [Fact]
        public void Resolve_PadRight_StringValidParameters_ShouldWork()
        {
            Assert.Equal("str00", Run("{PadRight(\"str\", 5, '0')}"));
        }

        [Fact]
        public void Resolve_PadRight_UsingDefaultPadChar_ShouldWork()
        {
            Assert.Equal("str  ", Run("{PadRight(\"str\", 5)}"));
        }

        [Fact]
        public void Resolve_PosetiveDecimal_ShouldProducePositiveDecimal()
        {
            Assert.Equal(new decimal(2.1), Run("{+2.1}"));
        }

        [Fact]
        public void Resolve_PosetiveInt_ShouldProducePositiveInt()
        {
            Assert.Equal(2, Run("{+2}"));
        }

        [Fact]
        public void Resolve_ScriptAddingFloatAndString_ShouldReturnStringThatCombinesBothValues()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"number", (float) 1.5}};
            // Run test
            var result = Run("{number +   \"kg\"}", variables);
            // Evaluate result
            Assert.Equal("1.5kg", result);
        }

        [Fact]
        public void Resolve_ScripWithStringEqualsToString_ShouldReturnFalse()
        {
            Assert.False((bool) Run("{\"str\"==\"str2\"}"));
        }

        [Fact]
        public void Resolve_StringEqualsToInt_ShouldReturnFalse()
        {
            Assert.False((bool) Run("{\"str\"==2}"));
        }

        [Fact]
        public void Resolve_StringEqualsToString_ShouldReturnTrue()
        {
            Assert.True((bool) Run("{\"str\"==\"str\"}"));
        }

        [Fact]
        public void Resolve_StringPlusVariable_ShouldReturnStringThatCombinesTheirValues()
        {
            // Setup test
            var variables = new Dictionary<string, object> {{"number", (decimal) 1.5}};
            // Run test
            var result = Run("{ \"value: \" + number}", variables);
            // Evaluate result
            Assert.Equal("value: 1.5", result);
        }

        [Fact]
        public void Resolve_StringValue_ShouldReturnStringValue()
        {
            Assert.Equal("str", Run("{\"str\"}"));
        }

        [Fact]
        public void Resolve_SubString_NegativeLengthValue_ShouldFail()
        {
            Assert.Throws<ArgumentException>(() => Run("{substring(\"substring\",1,-1)}"));
        }

        [Fact]
        public void Resolve_SubString_ValidParameters_ShouldWork()
        {
            Assert.Equal("su", Run("{substring(\"substring\",0,2)}"));
        }

        [Fact]
        public void Resolve_SubStringShouldFailOnNegativeStartValue()
        {
            Assert.Throws<ArgumentException>(() => Run("{substring(\"substring\",-1,2)}"));
        }

        [Fact]
        public void Resolve_TenDividedByTwo_ShouldReturnResult()
        {
            Assert.Equal(5, Run("{ 10/2}"));
        }

        [Fact]
        public void Resolve_TenMinusTwoTimesFive()
        {
            Assert.Equal(0, Run("{10-2*5 }"));
        }

        [Fact]
        public void Resolve_TenMinusTwoTimesFivePlusTwo_ShouldReturnResult()
        {
            Assert.Equal(2, Run("{10-2*5 +2}"));
        }

        [Fact]
        public void Resolve_TextBlock_ShouldReturnTextblockAsString()
        {
            Assert.Equal("abcdefg", Run("abcdefg"));
        }

        [Fact]
        public void Resolve_ThreeDotTwoPlusTwo_ShouldReturnDecimalResult()
        {
            Assert.Equal(new decimal(5.2), Run("{     3.2  +  2  }"));
        }

        [Fact]
        public void Resolve_ThreePlusTwoInBlockTimesFive_ShouldReturnResult()
        {
            Assert.Equal(25, Run("{     (3  +  2  ) *  5}"));
        }

        [Fact]
        public void Resolve_ThreeTimesOnePlusTwo_ShouldReturnResult()
        {
            Assert.Equal(5, Run("{3*1+2}"));
        }

        [Fact]
        public void Resolve_True_ShouldReturnBooleanTrue()
        {
            Assert.True((bool) Run("{true}"));
        }

        [Fact]
        public void Resolve_TwoGreaterThanOne_ShouldReturnTrue()
        {
            Assert.True((bool) Run("{2 > 1}"));
        }

        [Fact]
        public void Resolve_TwoLessThanOne_ShouldReturnTrue()
        {
            Assert.False((bool) Run("{2 < 1}"));
        }

        [Fact]
        public void Resolve_TwoLessThanOneCombinedWithIfElse_ShouldReturnFalsePart()
        {
            Assert.Equal(2, Run("{2 < 1 ? 1 : 2}"));
        }

        [Fact]
        public void Resolve_TwoTimesFiveTimesSeven_ShouldReturnResult()
        {
            Assert.Equal(70, Run("{ 2 * 5  * 7}"));
        }


        [Fact]
        public void Resolve_UnDefinedVariableWithNullDefaultValue1Dot2_ShouldThrowException()
        {
            Assert.Throws<VariableNotFoundException>(() => Run("{v1 ?? 1.2}"));
        }

        [Fact]
        public void Resolve_UnDefinedVariableWithUndefinedOrNullDefaultValue1Dot2_ShouldReturnDefaultValue()
        {
            var result = Run("{v1 ??? 1.2}");
            Assert.Equal(new decimal(1.2), result);
        }

        [Fact]
        public void Resolve_Variable_ShouldReturnVariableValue()
        {
            var variables = new Dictionary<string, object> {{"v1", 1}};
            Assert.Equal(1, Run("{v1}", variables));
        }

        [Fact]
        public void Resolve_VariableThatDoesNotExist_ShouldThrowAnException()
        {
            Assert.Throws<VariableNotFoundException>(() => Run("{v1}"));
        }
    }
}