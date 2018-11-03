using System;
using System.Collections.Generic;
using System.Text;
using Retypeit.Scripts.Bindings.Ast;
using Retypeit.Scripts.Bindings.Lexer;

namespace Retypeit.Scripts.Bindings.Parser
{
    public class TokenStream
    {
        public delegate bool ConsumeDelegate(ParserContext context, out IAstNode instr);

        public delegate bool ConsumeDelegate<TResult>(ParserContext context, out TResult result);

        private readonly List<Token> _tokens;

        private readonly Stack<int> _transactionPointers = new Stack<int>();
        private int _currentIndex;

        public TokenStream(ICollection<Token> tokens)
        {
            _tokens = new List<Token>(tokens);
            CurrentIndex = 0;
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            private set
            {
                _currentIndex = value;
                Current = GetToken(_currentIndex);
            }
        }

        public bool Eof => Current?.Type == TokenTypes.Eof;
        public Token Current { get; private set; }

        public Token Peek()
        {
            return GetToken(CurrentIndex + 1);
        }

        public void BeginTransaction()
        {
            _transactionPointers.Push(CurrentIndex);
        }

        public void Commit()
        {
            _transactionPointers.Pop();
        }

        public void Rollback()
        {
            CurrentIndex = _transactionPointers.Pop();
        }

        public bool Consume()
        {
            if (CurrentIndex == _tokens.Count - 1)
                return false;

            CurrentIndex++;
            return true;
        }

        public bool Consume(TokenTypes token)
        {
            if (Current.Type == token)
                return Consume();

            return false;
        }

        public bool Consume(TokenTypes token, out object tokenValue)
        {
            if (Current.Type == token)
            {
                tokenValue = Current.Value;
                return Consume();
            }

            tokenValue = null;
            return false;
        }

        public Token GetToken(int index)
        {
            if (index >= _tokens.Count) return new Token(TokenTypes.Eof);

            return _tokens[index];
        }

        public bool Consume(ConsumeDelegate consumer, ParserContext context, out IAstNode instr)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            BeginTransaction();
            if (consumer.Invoke(context, out instr))
            {
                Commit();
                return true;
            }

            Rollback();
            return false;
        }

        public bool Consume<TResult>(ConsumeDelegate<TResult> func, ParserContext context, out TResult result)
        {
            BeginTransaction();

            if (func.Invoke(context, out result))
            {
                Commit();
                return true;
            }

            Rollback();
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];
                if (i == CurrentIndex)
                    sb.AppendLine(token + "<--- current");
                else
                    sb.AppendLine(token.ToString());
            }

            return sb.ToString();
        }
    }
}