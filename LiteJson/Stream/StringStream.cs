using System.Text;

namespace LiteJson.Stream
{
    internal class StringStream : StreamUnstableBase<char>
    {
        public StringStream()
        {
        }

        public void PushString(string Value)
        {
            foreach (var Ch in Value)
            {
                Push(Ch);
            }
        }

        public override string ToString()
        {
            var VarString = new StringBuilder();

            for (var Index = 0; Index < Index_; ++Index)
            {
                VarString.Append(Buffer_[Index]);
            }

            return VarString.ToString();
        }

        public string ToString(int Index, int Count)
        {
            var VarString = new StringBuilder();

            for (var Offset = 0; Offset < Count; ++Offset)
            {
                if (Index + Offset >= Index_)
                {
                    break;
                }

                VarString.Append(Buffer_[Index + Offset]);
            }

            return VarString.ToString();
        }
    }
}