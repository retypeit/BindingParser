namespace Retypeit.Scripts.Bindings.Lexer
{
    public class Token
    {
        /// <summary>
        /// Used to identify if a value has been set or not
        /// </summary>
        private static readonly object NaN = new object();

        /// <summary>
        /// If the token has any specific value, ex: name, string etc..
        /// </summary>
        /// <value></value>
        public object Value { get; } = NaN;

        /// <summary>
        /// Type of token
        /// </summary>
        /// <value></value>
        public TokenTypes Type { get; }

        /// <summary>
        /// True if the token has been assigned a value
        /// </summary>
        public bool HasValue => Value != NaN;

        public Token(TokenTypes type)
        {
            Type = type;
            Value = NaN;
        }

        public Token(TokenTypes type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            if (Value == null)
                return $"{{\"Type\": {Type}}}";

            return $"{{\"Type\": {Type}, \"Value\": \"{Value}\"}}";
        }
    }
}