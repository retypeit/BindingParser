using System;
using System.Collections.Generic;
using System.Text;
using Retypeit.Scripts.Bindings.Interpreter;

namespace Retypeit.Scripts.Bindings.Extensions
{
    public class DictionaryVariableResolver : IVariableResolver
    {
        private readonly Dictionary<string, object> _target;

        public DictionaryVariableResolver(Dictionary<string, object> target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return _target.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out object value)
        {
            return _target.TryGetValue(key, out value);
        }
        
    }
}
