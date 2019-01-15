using System.Collections.Generic;
using System.Text;

namespace Retypeit.Scripts.Bindings.Lexer.Resolvers
{
    /// <summary>
    ///     Resolves a textblock (not part of an Interpolation block)
    /// </summary>
    public class TextBlockResolver : TokenResolverBase
    {
        private readonly BlockStyles _style;
        private const char CShartStartBlock = '{';
        private const string JavaScriptStartBlock = "!{";
        private const string LeftCurlyBracketExpression = "{{";

        public TextBlockResolver(BlockStyles style)
        {
            _style = style;
        }

        protected override bool DoTryResolve(CharStream stream, ICollection<Token> tokens)
        {
            if (stream.Eof)
                return false;

            if (_style == BlockStyles.CSharp)
            {
                if (stream.Current == '{')
                    return false;
            }
            else
            {
                if (stream.Peek(JavaScriptStartBlock))
                    return false;
            }


            var value = new StringBuilder();
            while (!stream.Eof)
            {
                var c = stream.Current;
                bool startBlock = false;
                if (_style == BlockStyles.CSharp)
                {
                    startBlock = c == CShartStartBlock;
                }
                else
                {
                    startBlock = stream.Peek(JavaScriptStartBlock);
                }

                if (startBlock)
                {
                    
                        if (_style == BlockStyles.CSharp &&  stream.TryMatch(LeftCurlyBracketExpression, false))
                        {
                            value.Append("{");
                            continue;
                        }

                    tokens.Add(new Token(TokenTypes.TextBlock, value.ToString()));
                    return true;
                }

                value.Append(c);
                stream.Read();
            }

            if (value.Length <= 0) return false;
            tokens.Add(new Token(TokenTypes.TextBlock, value.ToString()));
            return true;

        }
    }
}
