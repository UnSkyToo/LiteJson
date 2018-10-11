using System.Collections.Generic;

namespace LiteJson.Syntax
{
    internal class SyntaxObjectNode : SyntaxNode
    {
        private readonly List<string> Keys_;
        private readonly Dictionary<string, SyntaxNode> Childs_;

        public SyntaxObjectNode()
        {
            Keys_ = new List<string>();
            Childs_ = new Dictionary<string, SyntaxNode>();
        }

        public override SyntaxNodeType GetType()
        {
            return SyntaxNodeType.Object;
        }

        public bool AddChild(string Key, SyntaxNode Value)
        {
            if (Childs_.ContainsKey(Key))
            {
                return false;
            }
            Keys_.Add(Key);
            Childs_.Add(Key, Value);
            return true;
        }

        public string[] GetKeys()
        {
            return Keys_.ToArray();
        }

        public SyntaxNode GetValue(string Key)
        {
            return Childs_[Key];
        }
    }
}