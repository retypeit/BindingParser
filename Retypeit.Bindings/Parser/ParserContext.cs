using System;
using System.Collections.Generic;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Lexer;

namespace Retypeit.Scripts.Bindings.Parser
{
    public class ParserContext
    {

        public ParserContext(ICollection<Token> tokens, ICollection<FunctionInfo> validFunctions)
        {
            ValidFunctions= validFunctions ?? throw new ArgumentNullException(nameof(validFunctions));
            Stream = new TokenStream(tokens);
        }

        public TokenStream Stream { get; }
        public ICollection<FunctionInfo> ValidFunctions { get; }
    }
}