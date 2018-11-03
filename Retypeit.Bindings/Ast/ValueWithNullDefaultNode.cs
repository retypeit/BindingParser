using System;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class ValueWithNullDefaultNode : IAstNode
    {
        public ValueWithNullDefaultNode(IAstNode value, IAstNode defaultValue)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            DefaultValue = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
        }

        public IAstNode Value { get; }

        /// <summary>
        ///     Default value
        /// </summary>
        public IAstNode DefaultValue { get; }

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{Value} ?? {DefaultValue}";
        }
    }
}