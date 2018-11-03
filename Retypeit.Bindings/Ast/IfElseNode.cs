using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class IfElseNode : IAstNode
    {
        public IAstNode IfInstr { get; }
        public IAstNode ElseInstr { get; }

        public IAstNode Condition { get; }

        public IfElseNode(IAstNode condition, IAstNode ifInstr, IAstNode elseInstr)
        {
            Condition = condition;
            IfInstr = ifInstr;
            ElseInstr = elseInstr;
        }

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}