using System;
using System.Collections.Generic;
using System.Text;

namespace Retypeit.Scripts.Bindings.Interpreter
{
    public interface IVariableResolver
    {
        bool ContainsKey(string key);
        bool TryGetValue(string key, out object value);
    }
}
