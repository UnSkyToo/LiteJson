namespace LiteJson.Syntax
{
    internal class SyntaxLiteralBooleanNode : SyntaxLiteralNode
    {
        protected readonly bool Value_;

        public SyntaxLiteralBooleanNode(string Raw)
            : base(Raw)
        {
            Value_ = bool.Parse(Raw);
        }

        public bool GetValue()
        {
            return Value_;
        }
    }
}