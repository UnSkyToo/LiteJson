using LiteJson.Common;

namespace LiteJson.Stream
{
    internal class TokenStream : StreamStableBase<Token>
    {
        public TokenStream(Token[] Tokens)
        {
            Buffer_ = Tokens;
            Index_ = 0;
        }
    }
}