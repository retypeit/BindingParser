using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public abstract class MathNodeBase : IAstNode
    {
        private readonly string _operand;
        public IAstNode Value1 { get; }
        public IAstNode Value2 { get; }

        public MathNodeBase(IAstNode value1, IAstNode value2, string operand)
        {
            _operand = operand;
            Value1 = value1;
            Value2 = value2;
        }

        public abstract object Accept(IVisitor visitor);

        public override string ToString() => $"({Value1?.ToString() ?? "(null)"} {_operand} {Value2?.ToString() ?? "(null)"})";
    }
}