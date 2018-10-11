namespace LiteJson.Syntax
{
    internal class SyntaxLiteralNumericNode : SyntaxLiteralNode
    {
        protected readonly double Value_;

        public SyntaxLiteralNumericNode(string Raw)
            : base(Raw)
        {
            Value_ = double.Parse(Raw);
        }

        public double GetValue()
        {
            return Value_;
        }
    }
}