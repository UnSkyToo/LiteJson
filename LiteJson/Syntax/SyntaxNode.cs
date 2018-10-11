namespace LiteJson.Syntax
{
    internal enum SyntaxNodeType
    {
        Literal = 0,
        Array = 1,
        Object = 2,
    }

    internal abstract class SyntaxNode
    {
        public new abstract SyntaxNodeType GetType();
    }
}