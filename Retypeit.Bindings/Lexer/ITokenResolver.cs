using System.Collections.Generic;

namespace Retypeit.Scripts.Bindings.Lexer
{
    public interface ITokenResolver
    {
        bool TryResolve(CharStream stream, HashSet<Token> token);
    }
}