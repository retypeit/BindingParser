using System;
using System.Collections.Generic;
using System.Globalization;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Extensions;
using Retypeit.Scripts.Bindings.Interpreter;
using Retypeit.Scripts.Bindings.Lexer;
using Retypeit.Scripts.Bindings.Parser;

namespace Retypeit.Scripts.Bindings
{
    public class BindingResolver
    {
        private readonly BindingLexer _lexer;
        private readonly BindingParser _parser;
        private readonly object _workLock = new object();
        private CultureInfo _cultureInfo;
        private FunctionRunnerWrapper _functionMiddleware;
        private ICollection<FunctionInfo> _supportedFunctions;
        public BlockStyles BlockStyle { get; }

        public BindingResolver(BlockStyles blockStyle = BlockStyles.CSharp, IFunctionRunner runner = null, BindingLexer lexer = null, BindingParser parser = null,
            IAstCache cache = null)
        {
            _lexer = lexer ?? new BindingLexer(blockStyle);
            _parser = parser ?? new BindingParser();
            BlockStyle = blockStyle;
            Cache = cache;

            // Always setup standard functions
            UseRunner(new ObjectMethodRunner(new ScriptFunctions()));

            // Add custom functions
            if (runner != null)
                UseRunner(runner);
        }

        /// <summary>
        ///     Ast tree cache
        /// </summary>
        public IAstCache Cache { get; set; }

        /// <summary>
        ///     Culture info used when converting values to string
        /// </summary>
        public CultureInfo CultureInfo
        {
            get => _cultureInfo ?? CultureInfo.CurrentCulture;
            set => _cultureInfo = value;
        }

        /// <summary>
        ///     Functions that this resolver supports
        /// </summary>
        public ICollection<FunctionInfo> SupportedFunctions
        {
            get
            {
                lock (_workLock)
                {
                    if (_supportedFunctions != null)
                        return _supportedFunctions;

                    _supportedFunctions = _functionMiddleware.ListSupportedFunctions();
                    return _supportedFunctions;
                }
            }
        }

        /// <summary>
        ///     Adds a middleware that manages the execution of functions requested by the script
        /// </summary>
        /// <param name="runner"></param>
        public void UseRunner(IFunctionRunner runner)
        {
            if (runner == null)
                throw new ArgumentNullException(nameof(runner));

            lock (_workLock)
            {
                _supportedFunctions = null;
                _functionMiddleware = new FunctionRunnerWrapper(runner, _functionMiddleware);
            }
        }

        /// <summary>
        ///     Resolves strings containing interpolation scripts
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public object Resolve(string expression, Dictionary<string, object> variables = null)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var cache = Cache; // Store in local variable in-case another thread changes the value
            AstRoot root;

            if (cache == null)
            {
                var tokens = _lexer.Scan(expression);
                root = _parser.Parse(tokens, _functionMiddleware.ListSupportedFunctions());
            }
            else
            {
                root = cache.GetOrAdd(expression, expr =>
                {
                    // Create the nodes that should be stored and returned by the cache
                    var tokens = _lexer.Scan(expr);
                    var node = _parser.Parse(tokens, _functionMiddleware.ListSupportedFunctions());
                    return node;
                });

                if (root == null)
                    throw new BindingResolverException(
                        $"The cache {cache.GetType().Name} did not return any instruction to run!");
            }

            var visitor = new ScriptVisitor(context => _functionMiddleware.Invoke(context), variables, CultureInfo);
            return visitor.Visit(root);
        }
    }
}