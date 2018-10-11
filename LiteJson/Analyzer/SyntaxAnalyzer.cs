using LiteJson.Common;
using LiteJson.Stream;
using LiteJson.Syntax;

namespace LiteJson.Analyzer
{
    internal class SyntaxAnalyzer : IAnalyzer
    {
        private readonly TokenStream TokensStream_;
        private SyntaxNode JsonNode_;

        public SyntaxAnalyzer(TokenStream TokensStream)
        {
            TokensStream_ = TokensStream;
        }

        public SyntaxNode GetJsonNode()
        {
            return JsonNode_;
        }

        public bool Analyzing()
        {
            JsonNode_ = ParseChunk();
            if (JsonNode_ == null)
            {
                return false;
            }

            if (TokensStream_.IsEnd())
            {
                return true;
            }

            return false;
        }

        private SyntaxNode ParseChunk()
        {
            var CurrentToken = TokensStream_.Peek();

            if (CurrentToken.Code_ == LanguageDescriptor.JsonObjectBegin)
            {
                return ParseObject();
            }
            if (CurrentToken.Code_ == LanguageDescriptor.JsonArrayBegin)
            {
                return ParseArray();
            }
            Logger.Add(CurrentToken.Line_, CurrentToken.Index_, $"unexpected token {CurrentToken.Code_}");
            return null;
        }

        private SyntaxObjectNode ParseObject()
        {
            if (TokensStream_.Take().Code_ != LanguageDescriptor.JsonObjectBegin)
            {
                Logger.Add(TokensStream_.Peek().Line_, TokensStream_.Peek().Index_, $"expected '{LanguageDescriptor.JsonObjectBegin}' not appeared");
                return null;
            }

            var Node = new SyntaxObjectNode();
            while (!TokensStream_.IsEnd())
            {
                if (TokensStream_.Peek().Code_ == LanguageDescriptor.JsonObjectEnd)
                {
                    break;
                }

                var Key = ParseKey();
                if (string.IsNullOrWhiteSpace(Key))
                {
                    return null;
                }

                if (TokensStream_.Take().Code_ != LanguageDescriptor.JsonDefineDelimiter)
                {
                    Logger.Add(TokensStream_.Peek().Line_, TokensStream_.Peek().Index_, $"expected '{LanguageDescriptor.JsonDefineDelimiter}' not appeared");
                    return null;
                }

                var Value = ParseValue();
                if (Value == null)
                {
                    return null;
                }

                Node.AddChild(Key, Value);

                if (TokensStream_.Peek().Code_ == LanguageDescriptor.JsonValueDelimiter)
                {
                    TokensStream_.Take();
                }
            }

            TokensStream_.Take();
            return Node;
        }

        private SyntaxArrayNode ParseArray()
        {
            if (TokensStream_.Take().Code_ != LanguageDescriptor.JsonArrayBegin)
            {
                Logger.Add(TokensStream_.Peek().Line_, TokensStream_.Peek().Index_, $"expected '{LanguageDescriptor.JsonArrayBegin}' not appeared");
                return null;
            }

            var Node = new SyntaxArrayNode();
            while (!TokensStream_.IsEnd())
            {
                if (TokensStream_.Peek().Code_ == LanguageDescriptor.JsonArrayEnd)
                {
                    break;
                }

                var Value = ParseValue();
                if (Value == null)
                {
                    return null;
                }

                Node.AddValue(Value);

                if (TokensStream_.Peek().Code_ == LanguageDescriptor.JsonValueDelimiter)
                {
                    TokensStream_.Take();
                }
            }

            TokensStream_.Take();
            return Node;
        }

        private string ParseKey()
        {
            var Tok = TokensStream_.Take();
            if (Tok.Type_ == TokenType.String)
            {
                return Tok.Code_;
            }
            Logger.Add(Tok.Line_, Tok.Index_, $"unexpected token {Tok.Code_}");
            return string.Empty;
        }

        private SyntaxNode ParseValue()
        {
            var Tok = TokensStream_.Take();
            if (Tok.Type_ == TokenType.Delimiter)
            {
                TokensStream_.Back();
                return ParseChunk();
            }
            if (Tok.Type_ == TokenType.Null)
            {
                return new SyntaxLiteralNullNode(Tok.Code_);
            }
            if (Tok.Type_ == TokenType.Boolean)
            {
                return new SyntaxLiteralBooleanNode(Tok.Code_);
            }
            if (Tok.Type_ == TokenType.Numeric)
            {
                return new SyntaxLiteralNumericNode(Tok.Code_);
            }
            if (Tok.Type_ == TokenType.String)
            {
                return new SyntaxLiteralStringNode(Tok.Code_);
            }
            
            Logger.Add(Tok.Line_, Tok.Index_, $"unexpected token {Tok.Code_}");
            return null;
        }
    }
}