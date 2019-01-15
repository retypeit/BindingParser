using System;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class AstRoot : IAstNode
    {
        public IAstNode Child { get; }

        public AstRoot(IAstNode child)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }

        public object Accept(IVisitor visitor)
        {
            return Child.Accept(visitor);
        }
    }
}