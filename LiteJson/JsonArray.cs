using System.Collections.Generic;

namespace LiteJson
{
    public class JsonArray
    {
        private readonly List<JsonValue> Values_;

        public JsonValue this[int Index]
        {
            get
            {
                if (Index >= 0 && Index < Values_.Count)
                {
                    return Values_[Index];
                }

                return null;
            }
        }

        public JsonArray()
        {
            Values_ = new List<JsonValue>();
        }
        
        public void Clear()
        {
            Values_.Clear();
        }

        public void Add(JsonValue Value)
        {
            Values_.Add(Value);
        }

        public void Remove(JsonValue Value)
        {
            Values_.Remove(Value);
        }
        
        public void RemoveAt(int Index)
        {
            if (Index >= 0 && Index < Values_.Count)
            {
                Values_.RemoveAt(Index);
            }
        }
    }
}