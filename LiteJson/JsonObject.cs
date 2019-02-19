using System.Collections;
using System.Collections.Generic;

namespace LiteJson
{
    public class JsonObject : IEnumerable<KeyValuePair<string, JsonValue>>
    {
        private readonly Dictionary<string, JsonValue> Values_;

        public JsonValue this[string Key] => Values_.ContainsKey(Key) ? Values_[Key] : null;

        public JsonObject()
        {
            Values_ = new Dictionary<string, JsonValue>();
        }

        public override string ToString()
        {
            return $"[JsonObject]";
        }

        public override int GetHashCode()
        {
            return Values_.GetHashCode();
        }

        public void SetValue(string Key, JsonValue Value)
        {
            if (Values_.ContainsKey(Key))
            {
                Values_[Key] = Value;
            }
            else
            {
                Values_.Add(Key, Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var Value in Values_)
            {
                yield return Value;
            }
        }

        IEnumerator<KeyValuePair<string, JsonValue>> IEnumerable<KeyValuePair<string, JsonValue>>.GetEnumerator()
        {
            foreach (var Value in Values_)
            {
                yield return Value;
            }
        }
    }
}