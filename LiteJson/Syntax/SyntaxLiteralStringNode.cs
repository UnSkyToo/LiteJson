namespace LiteJson.Syntax
{
    internal class SyntaxLiteralStringNode : SyntaxLiteralNode
    {
        protected readonly string Value_;

        public SyntaxLiteralStringNode(string Raw)
            : base(Raw)
        {
            Value_ = Raw;
        }

        public string GetValue()
        {
            return Value_;
        }
    }
}