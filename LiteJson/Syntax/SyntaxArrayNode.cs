using System.Collections.Generic;

namespace LiteJson.Syntax
{
    internal class SyntaxArrayNode : SyntaxNode
    {
        protected readonly List<SyntaxNode> Values_;

        public SyntaxArrayNode()
        {
            Values_ = new List<SyntaxNode>();
        }

        public override SyntaxNodeType GetType()
        {
            return SyntaxNodeType.Array;
        }

        public void AddValue(SyntaxNode Node)
        {
            Values_.Add(Node);
        }

        public SyntaxNode[] GetValues()
        {
            return Values_.ToArray();
        }
    }
}