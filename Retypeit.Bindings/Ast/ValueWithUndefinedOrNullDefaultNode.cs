using System;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class ValueWithUndefinedOrNullDefaultNode : IAstNode
    {
        public IAstNode Value { get; }
        /// <summary>
        /// Default value
        /// </summary>
        public IAstNode DefaultValue { get; }

        public ValueWithUndefinedOrNullDefaultNode(IAstNode value, IAstNode defaultValue)
        {
            
            Value = value ?? throw new ArgumentNullException(nameof(value));
            DefaultValue = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
        }

        public override string ToString() => $"{Value} ?? {DefaultValue}";

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
