namespace LiteJson.Syntax
{
    internal class SyntaxLiteralNode : SyntaxNode
    {
        protected readonly string Raw_;

        public SyntaxLiteralNode(string Raw)
        {
            Raw_ = Raw;
        }

        public override SyntaxNodeType GetType()
        {
            return SyntaxNodeType.Literal;
        }

        public string GetRaw()
        {
            return Raw_;
        }
    }
}