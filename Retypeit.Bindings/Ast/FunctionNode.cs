using System.Collections.Generic;
using System.Linq;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Ast
{
    public class FunctionNode : IAstNode
    {
        public ICollection<IAstNode> Parameters { get; }
        public string Name { get; }

        public FunctionNode(string name, List<IAstNode> parameters)
        {
            Parameters = parameters;
            Name = name;
        }

        public override string ToString() => $"{Name}({string.Join(",", Parameters.Select(i => i.ToString()))}";

        public object Accept(IVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}