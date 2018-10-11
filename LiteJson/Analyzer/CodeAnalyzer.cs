using LiteJson.Stream;
using LiteJson.Syntax;

namespace LiteJson.Analyzer
{
    internal class CodeAnalyzer : IAnalyzer
    {
        private readonly SyntaxNode JsonNode_;
        private readonly StringStream JsonString_;
        private int ChunkLevel_;

        public CodeAnalyzer(SyntaxNode JsonNode)
        {
            JsonNode_ = JsonNode;
            JsonString_ = new StringStream();
        }

        public string GetJsonString()
        {
            return JsonString_.ToString();
        }

        public bool Analyzing()
        {
            JsonString_.Reset();
            ChunkLevel_ = 0;
            return WriteSyntaxNode(JsonNode_);
        }

        private bool WriteSyntaxNode(SyntaxNode Node)
        {
            if (Node.GetType() == SyntaxNodeType.Literal)
            {
                return WriteSyntaxLiteralNode(Node as SyntaxLiteralNode);
            }
            if (Node.GetType() == SyntaxNodeType.Array)
            {
                return WriteSyntaxArrayNode(Node as SyntaxArrayNode);
            }
            if (Node.GetType() == SyntaxNodeType.Object)
            {
                return WriteSyntaxObjectNode(Node as SyntaxObjectNode);
            }
            return false;
        }

        private bool WriteSyntaxLiteralNode(SyntaxLiteralNode Node)
        {
            if (Node is SyntaxLiteralNullNode)
            {
                JsonString_.PushString((Node as SyntaxLiteralNullNode).GetRaw());
            }
            else if (Node is SyntaxLiteralBooleanNode)
            {
                JsonString_.PushString((Node as SyntaxLiteralBooleanNode).GetRaw());
            }
            else if (Node is SyntaxLiteralNumericNode)
            {
                JsonString_.PushString((Node as SyntaxLiteralNumericNode).GetRaw());
            }
            else if (Node is SyntaxLiteralStringNode)
            {
                JsonString_.Push('"');
                JsonString_.PushString((Node as SyntaxLiteralStringNode).GetRaw());
                JsonString_.Push('"');
            }

            return true;
        }

        private bool WriteSyntaxArrayNode(SyntaxArrayNode Node)
        {
            EnterChunk();
            JsonString_.Push('[');
            JsonString_.Push('\n');

            var Childs = Node.GetValues();

            for (var Index = 0; Index < Childs.Length; ++Index)
            {
                WriteChunkTab();
                WriteSyntaxNode(Childs[Index]);
                if (Index != Childs.Length - 1)
                {
                    JsonString_.Push(',');
                }
                JsonString_.Push('\n');
            }

            LeaveChunk();
            WriteChunkTab();
            JsonString_.Push(']');
            return true;
        }

        private bool WriteSyntaxObjectNode(SyntaxObjectNode Node)
        {
            EnterChunk();
            JsonString_.Push('{');
            JsonString_.Push('\n');

            var Keys = Node.GetKeys();
            for (var Index = 0; Index < Keys.Length; ++Index)
            {
                var Child = Node.GetValue(Keys[Index]);
                WriteChunkTab();
                JsonString_.Push('"');
                JsonString_.PushString(Keys[Index]);
                JsonString_.Push('"');
                JsonString_.PushString(" : ");
                WriteSyntaxNode(Child);
                if (Index != Keys.Length - 1)
                {
                    JsonString_.Push(',');
                }
                JsonString_.Push('\n');
            }

            LeaveChunk();
            WriteChunkTab();
            JsonString_.Push('}');
            return true;
        }

        private void EnterChunk()
        {
            ChunkLevel_++;
        }

        private void LeaveChunk()
        {
            ChunkLevel_--;
        }

        private void WriteChunkTab()
        {
            for (var Index = 0; Index < ChunkLevel_; ++Index)
            {
                JsonString_.Push('\t');
            }
        }
    }
}