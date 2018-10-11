namespace LiteJson.Stream
{
    internal class CharacterStream : StreamStableBase<char>
    {
        public CharacterStream(string Source)
        {
            Buffer_ = Source.ToCharArray();
            Index_ = 0;
        }
    }
}