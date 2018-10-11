using System.Collections.Generic;

namespace LiteJson
{
    public class JsonObject
    {
        private readonly Dictionary<string, JsonValue> Values_;

        public JsonValue this[string Key] => Values_.ContainsKey(Key) ? Values_[Key] : null;

        public JsonObject()
        {
            Values_ = new Dictionary<string, JsonValue>();
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
    }
}