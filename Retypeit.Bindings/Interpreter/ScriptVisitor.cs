using System;
using System.Collections.Generic;
using System.Globalization;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Exceptions;
using Retypeit.Scripts.Bindings.ExtensionMethods;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Helpers;

namespace Retypeit.Scripts.Bindings.Interpreter
{
    /// <summary>
    ///     Visitor used to implement the binding language
    /// </summary>
    internal class ScriptVisitor : IVisitor
    {
        private readonly CultureInfo _culture;

        public ScriptVisitor(Func<FunctionContext, object> functionRunner, Dictionary<string, object> variables = null,
            CultureInfo culture = null)
        {
            _functionRunner = functionRunner ?? throw new ArgumentNullException(nameof(functionRunner));
            _variables = variables ?? new Dictionary<string, object>();
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        private readonly Func<FunctionContext, object> _functionRunner;
        private readonly Dictionary<string, object> _variables;

        public object Visit(VariableNode node)
        {
            if (!_variables.TryGetValue(node.Identity, out var value))
                throw new VariableNotFoundException(node.Identity, $"Variable [{node.Identity}] does not exist");

            return value;
        }

        /// <summary>
        ///     Will return the default value if the variable is not defined or its value is null
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public object Visit(ValueWithNullDefaultNode node)
        {
            return node.Value.Accept(this) ?? node.DefaultValue.Accept(this);
        }

        public object Visit(ValueWithUndefinedOrNullDefaultNode node)
        {
            if (node.Value is VariableNode variableNode)
                if (!_variables.ContainsKey(variableNode.Identity))
                    return node.DefaultValue.Accept(this);

            return node.Value.Accept(this) ?? node.DefaultValue.Accept(this);
        }

        public object Visit(ValueNode node)
        {
            return node.Value;
        }

        public object Visit(SubNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 - v2,
                (v1, v2) => v1 - v2,
                (v1, v2, ex) =>
                    throw new InvalidOperationException($"[{v1 ?? "(null)"} - {v2 ?? "(null)"}] failed!", ex));
        }

        public object Visit(MultiplyNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 * v2,
                (v1, v2) => v1 * v2,
                (v1, v2, ex) =>
                    throw new InvalidOperationException($"[{v1 ?? "(null)"} * {v2 ?? "(null)"}] failed!", ex));
        }

        public object Visit(IfElseNode node)
        {
            var condition = node.Condition.Accept(this);
            if (condition is bool bvalue)
                return bvalue ? node.IfInstr.Accept(this) : node.ElseInstr.Accept(this);

            throw new Exception("A condition must return a boolean value");
        }

        public object Visit(FunctionNode node)
        {
            var paramList = new List<object>();
            foreach (var pi in node.Parameters) paramList.Add(pi.Accept(this));
            return _functionRunner.Invoke(new FunctionContext(node.Name, paramList));
        }

        public object Visit(NotEqualNode node)
        {
            return !IsEqual(node.Value1, node.Value2);
        }

        public object Visit(EqualNode node)
        {
            return IsEqual(node.Value1, node.Value2);
        }

        public object Visit(BoolValueNode node)
        {
            return node.Value;
        }

        public object Visit(GreaterThanNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 > v2,
                (v1, v2) => v1 > v2,
                (v1, v2, ex) =>
                    throw new InvalidOperationException($"[{v1 ?? "(null)"} > {v2 ?? "(null)"}] failed!", ex));
        }

        public object Visit(LessThanNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 < v2,
                (v1, v2) => v1 < v2,
                (v1, v2, ex) =>
                    throw new InvalidOperationException($"[{v1 ?? "(null)"} > {v2 ?? "(null)"}] failed!", ex));
        }

        public object Visit(DivideNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 / v2,
                (v1, v2) => v1 / v2,
                (v1, v2, ex) =>
                    throw new InvalidOperationException($"[{v1 ?? "(null)"} / {v2 ?? "(null)"}] failed!", ex));
        }

        public object Visit(AddNode node)
        {
            return this.Calculate(node.Value1, node.Value2,
                (v1, v2) => v1 + v2,
                (v1, v2) => v1 + v2,
                HandleError);

            object HandleError(object v1, object v2, Exception exception)
            {
                if (v1 is string || v2 is string)
                    return ValueHelper.ToString(v1, _culture) + ValueHelper.ToString(v2, _culture);

                throw new InvalidOperationException($"[{v1 ?? "(null)"} + {v2 ?? "(null)"}] failed!", exception);
            }
        }

        public object Visit(AstRoot node)
        {
            return node.Accept(this);
        }


        private bool IsEqual(IAstNode value1, IAstNode value2)
        {
            var v1 = value1.Accept(this);
            var v2 = value2.Accept(this);

            if (v1 == null && v2 == null) return true;

            if (v1 == null || v2 == null) return false;

            if (ValueHelper.IsNumeric(v1) && ValueHelper.IsNumeric(v2))
            {
                var d1 = Convert.ToDecimal(v1);
                var d2 = Convert.ToDecimal(v2);
                return d1 == d2;
            }

            return v1.Equals(v2);
        }
    }
}