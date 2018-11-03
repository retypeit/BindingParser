using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class ValueNode : IAstNode
    {
        public object Value { get; }

        public ValueNode(object value)
        {
            Value = value;
        }

        public override string ToString() => Value?.ToString() ?? "(null)";

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}