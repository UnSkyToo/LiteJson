namespace LiteJson.Common
{
    internal enum TokenType
    {
        Identity = 0,
        Null = 1,
        Boolean = 2,
        Numeric = 3,
        String = 4,
        Delimiter = 5,

        None = 254,
        Error = 255,
    }

    internal class Token
    {
        public TokenType Type_;
        public string Code_;
        public int Line_;
        public int Index_;

        public Token(TokenType Type, string Code, int Line, int Index)
        {
            Type_ = Type;
            Code_ = Code;
            Line_ = Line;
            Index_ = Index;
        }
    }
}