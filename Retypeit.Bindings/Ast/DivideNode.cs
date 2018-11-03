using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class DivideNode : MathNodeBase
    {
        public DivideNode(IAstNode value1, IAstNode value2) : base(value1, value2, "/")
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}