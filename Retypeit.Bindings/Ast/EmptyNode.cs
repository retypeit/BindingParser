using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class EmptyNode : IBooleanResultNode
    {
        private const string Result = "";

        public object Accept(IVisitor visitor)
        {
            return Result;
        }
    }
}