using System;
using Retypeit.Scripts.Bindings.Ast;

namespace Retypeit.Scripts.Bindings.Extensions
{
    /// <summary>
    ///     Describes a class that caches the parsed trees of scripts
    ///     to enable optimization by not forcing the system to re lex and parse strings.
    /// </summary>
    public interface IAstCache
    {
        /// <summary>
        ///     Get a cached AST tree or create and cache one
        /// </summary>
        /// <param name="script"></param>
        /// <param name="astFactory"></param>
        /// <returns></returns>
        AstRoot GetOrAdd(string script, Func<string, AstRoot> astFactory);
    }
}