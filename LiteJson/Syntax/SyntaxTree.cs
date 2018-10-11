namespace LiteJson.Syntax
{
    internal class SyntaxTree
    {
        private readonly SyntaxNode Root_;

        public SyntaxTree(SyntaxNode Root)
        {
            Root_ = Root;
        }

        public SyntaxNode GetRoot()
        {
            return Root_;
        }
    }
}