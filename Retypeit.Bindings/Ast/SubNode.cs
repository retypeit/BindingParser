using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class SubNode : MathNodeBase
    {
        public SubNode(IAstNode value1, IAstNode value2) : base(value1, value2, "-")
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}