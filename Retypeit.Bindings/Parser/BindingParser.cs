using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Exceptions;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Lexer;

namespace Retypeit.Scripts.Bindings.Parser
{
    public class BindingParser
    {
        public AstRoot Parse(ICollection<Token> tokens, ICollection<FunctionInfo> validFunctions)
        {
            if (tokens == null) throw new ArgumentNullException(nameof(tokens));
            IAstNode program = null;
            var context = new ParserContext(tokens, validFunctions);

            while (!context.Stream.Eof)
            {
                IAstNode currInstr;
                if (TryParseTextBlock(context, out var textBlockInstr))
                    currInstr = textBlockInstr;
                else if (TryParseBindingBlock(context, out var stmtInstr))
                    currInstr = stmtInstr;
                else
                    throw new ParserException("Syntax error!", context.Stream);

                if (program != null)
                    program = new AddNode(program, currInstr);
                else
                    program = currInstr;
            }

            return new AstRoot(program ?? new EmptyNode());
        }

        private bool TryParseTextBlock(ParserContext context, out IAstNode instr)
        {
            if (context.Stream.Consume(TokenTypes.TextBlock, out var value))
            {
                instr = new ValueNode(value);
                return true;
            }

            instr = null;
            return false;
        }

        private bool TryParseBindingBlock(ParserContext context, out IAstNode instr)
        {
            instr = null;

            if (!context.Stream.Consume(TokenTypes.LeftBracket))
                return false;

            if (!TryStatement(context, out instr))
                return false;

            if (context.Stream.Consume(TokenTypes.RightBracket))
                return true;

            return false;
        }

        private bool TryStatement(ParserContext context, out IAstNode instr)
        {
            instr = null;

            if (context.Stream.Consume<IAstNode>(TryCondition, context, out var condition))
            {
                if (context.Stream.Consume(TokenTypes.QuestionMark))
                {
                    if (!context.Stream.Consume<IAstNode>(TryExpression, context, out var value1))
                        return false;
                    if (!context.Stream.Consume(TokenTypes.Colon))
                        return false;
                    if (!context.Stream.Consume<IAstNode>(TryExpression, context, out var value2))
                        return false;

                    instr = new IfElseNode(condition, value1, value2);

                    return true;
                }

                instr = condition;
                return true;
            }

            if (context.Stream.Consume<IAstNode>(TryExpression, context, out instr)) return true;

            return false;
        }

        private bool TryCondition(ParserContext context, out IAstNode instr)
        {
            if (context.Stream.Consume<IAstNode>(TryExpression, context, out var currInstr) &&
                currInstr is IBooleanResultNode)
            {
                instr = currInstr;
                return true;
            }

            instr = null;
            return false;
        }

        private bool TryExpression(ParserContext context, out IAstNode instr)
        {
            IAstNode value2;
            if (context.Stream.Consume<IAstNode>(TryTerm, context, out var currInstr))
            {
                instr = currInstr;

                while (!context.Stream.Eof)
                    if (context.Stream.Consume(TokenTypes.Plus))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new AddNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.Minus))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new SubNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.GreaterThan))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new GreaterThanNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.LessThan))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new LessThanNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.Equal))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new EqualNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.NotEqual))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryTerm, context, out value2))
                            return false;

                        currInstr = new NotEqualNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else
                    {
                        return true;
                    }

                return true;
            }

            instr = null;
            return false;
        }

        private bool TryTerm(ParserContext context, out IAstNode instr)
        {
            IAstNode value2;
            if (context.Stream.Consume<IAstNode>(TryFactor, context, out var currInstr))
            {
                instr = currInstr;

                while (!context.Stream.Eof)
                    if (context.Stream.Consume(TokenTypes.Times))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryFactor, context, out value2))
                            throw new ParserException("Syntax error: expected factor", context.Stream);

                        currInstr = new MultiplyNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else if (context.Stream.Consume(TokenTypes.Divide))
                    {
                        if (!context.Stream.Consume<IAstNode>(TryFactor, context, out value2))
                            throw new ParserException("Syntax error: expected factor", context.Stream);

                        currInstr = new DivideNode(currInstr, value2);
                        instr = currInstr;
                    }
                    else
                    {
                        return true;
                    }

                return true;
            }

            instr = null;
            return false;
        }

        private bool TryParameters(ParserContext context, out List<IAstNode> parameters)
        {
            parameters = new List<IAstNode>();
            if (!context.Stream.Consume<IAstNode>(TryStatement, context, out var parameter))
                return true;

            parameters.Add(parameter);

            while (context.Stream.Current.Type != TokenTypes.Eof)
            {
                if (!context.Stream.Consume(TokenTypes.Comma))
                    return true;
                if (!context.Stream.Consume<IAstNode>(TryStatement, context, out parameter))
                    return false;

                parameters.Add(parameter);
            }

            return false;
        }

        private bool TryFactor(ParserContext context, out IAstNode instr)
        {
            if (context.Stream.Consume<IAstNode>(TryValueOrNullDefault, context, out instr))
                return true;

            if (context.Stream.Consume<IAstNode>(TryValueOrUndefinedOrNullDefault, context, out instr))
                return true;

            if (context.Stream.Consume<IAstNode>(TryValue, context, out instr))
                return true;

            return false;
        }

        private bool TryValueOrNullDefault(ParserContext context, out IAstNode instr)
        {
            instr = null;

            if (!context.Stream.Consume<IAstNode>(TryValue, context, out var value))
                return false;

            if (!context.Stream.Consume(TokenTypes.DefaultNullValue))
                return false;

            if (!context.Stream.Consume<IAstNode>(TryFactor, context, out var defaultValue))
                return false;

            instr = new ValueWithNullDefaultNode(value, defaultValue);
            return true;
        }

        private bool TryValueOrUndefinedOrNullDefault(ParserContext context, out IAstNode instr)
        {
            instr = null;

            if (!context.Stream.Consume<IAstNode>(TryValue, context, out var value))
                return false;

            if (!context.Stream.Consume(TokenTypes.DefaultUndefinedOrNullValue))
                return false;

            if (!context.Stream.Consume<IAstNode>(TryFactor, context, out var defaultValue))
                return false;

            instr = new ValueWithUndefinedOrNullDefaultNode(value, defaultValue);
            return true;
        }

        private bool TryValue(ParserContext context, out IAstNode instr)
        {
            instr = null;

            if (context.Stream.Consume(TokenTypes.Identity, out var value))
            {
                if (context.Stream.Consume(TokenTypes.LeftParentheses))
                {
                    // We have parentheses = must be a function
                    if (!context.Stream.Consume<List<IAstNode>>(TryParameters, context, out var parameters))
                        throw new ParserException("Syntax error", context.Stream);
                    if (!context.Stream.Consume(TokenTypes.RightParentheses))
                        throw new ParserException("Syntax error, expected )", context.Stream);

                    var functionName = value.ToString();

                    // Validate that the function exists
                    var matchingFunctions = context.ValidFunctions.Where(i =>
                        i.Name.Equals(functionName, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (!matchingFunctions.Any())
                        throw new ParserException($"Unknown function: {functionName}", context.Stream);

                    // Validate that we have a function that has the same number of parameters 
                    // as was entered in the script
                    if (matchingFunctions.All(i => i.Parameters.Count != parameters.Count))
                    {
                        var errMsg = new StringBuilder();
                        errMsg.AppendLine("Invalid number of parameters");
                        errMsg.AppendLine("Supported: ");
                        foreach (var func in matchingFunctions)
                            errMsg.AppendLine(
                                $"  * {func.Name}({string.Join(",", func.Parameters.Select(i => i.Name))})");
                        throw new ParserException(errMsg.ToString(), context.Stream);
                    }

                    instr = new FunctionNode(functionName, parameters);


                    return true;
                }

                // Missing parentheses = must be a bound variable.
                instr = new VariableNode(value?.ToString());
                return true;
            }

            if (context.Stream.Consume(TokenTypes.String, out value))
            {
                if (value == null)
                    instr = new ValueNode(null);
                else
                    instr = new ValueNode(Regex.Unescape(value.ToString()));
                return true;
            }

            if (context.Stream.Consume(TokenTypes.Integer, out value) ||
                context.Stream.Consume(TokenTypes.Decimal, out value) ||
                context.Stream.Consume(TokenTypes.Char, out value))
            {
                instr = new ValueNode(value);
                return true;
            }

            if (context.Stream.Consume(TokenTypes.LeftParentheses))
            {
                if (!context.Stream.Consume<IAstNode>(TryStatement, context, out instr))
                    throw new ParserException("Syntax error", context.Stream);

                if (!context.Stream.Consume(TokenTypes.RightParentheses))
                    throw new ParserException("Syntax error, expected )", context.Stream);

                return true;
            }

            if (context.Stream.Consume(TokenTypes.Plus))
            {
                if (!context.Stream.Consume<IAstNode>(TryFactor, context, out instr))
                    throw new ParserException("Syntax error", context.Stream);
                return true;
            }

            if (context.Stream.Consume(TokenTypes.Minus))
            {
                if (!context.Stream.Consume<IAstNode>(TryFactor, context, out var factor))
                    throw new ParserException("Syntax error", context.Stream);
                instr = new MultiplyNode(new ValueNode(-1), factor);
                return true;
            }

            if (context.Stream.Consume(TokenTypes.Null))
            {
                instr = new ValueNode(null);
                return true;
            }

            if (context.Stream.Consume(TokenTypes.True))
            {
                instr = new BoolValueNode(true);
                return true;
            }

            if (context.Stream.Consume(TokenTypes.False))
            {
                instr = new BoolValueNode(false);
                return true;
            }

            return false;
        }
    }
}