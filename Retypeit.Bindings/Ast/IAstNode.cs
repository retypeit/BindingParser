using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public interface IAstNode
    {
        object Accept(IVisitor visitor);
    }
}