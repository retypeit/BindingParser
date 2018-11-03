using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class VariableNode : IAstNode
    {
        public string Identity { get; }

        public VariableNode(string identity)
        {
            Identity = identity;
        }

        public override string ToString() => Identity;

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}