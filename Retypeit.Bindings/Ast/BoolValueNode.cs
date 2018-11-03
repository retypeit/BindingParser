using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class BoolValueNode : IBooleanResultNode
    {
        public bool Value { get; }

        public BoolValueNode(bool value)
        {
            Value = value;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
