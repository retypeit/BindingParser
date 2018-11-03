using Retypeit.Scripts.Bindings.Ast;

namespace Retypeit.Scripts.Bindings.Interpreter
{
    public interface IVisitor
    {
        object Visit(VariableNode node);

        object Visit(ValueNode node);

        object Visit(SubNode node);

        object Visit(NotEqualNode node);

        object Visit(MultiplyNode node);

        object Visit(IfElseNode node);

        object Visit(FunctionNode node);

        object Visit(EqualNode node);
        object Visit(BoolValueNode node);
        object Visit(GreaterThanNode node);
        object Visit(LessThanNode node);

        object Visit(DivideNode node);

        object Visit(AddNode node);

        object Visit(AstRoot node);
        object Visit(ValueWithNullDefaultNode variableWithDefaultNode);

        object Visit(ValueWithUndefinedOrNullDefaultNode node);
    }
}