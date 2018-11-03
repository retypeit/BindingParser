using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class GreaterThanNode : IBooleanResultNode
    {
        public GreaterThanNode(IAstNode value1, IAstNode value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public IAstNode Value1 { get; }
        public IAstNode Value2 { get; }

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}