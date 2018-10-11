using System;

namespace LiteJson
{
    public enum JsonValueType
    {
        Boolean = 1,
        String  = 2,
        Numeric = 3,
        Array   = 4,
        Object  = 5,
    }

    public class JsonValue
    {
        private JsonValueType ValueType { get; set; }
        private object Value { get; set; }

        public JsonValue(JsonValueType JValueType, object JValue)
        {
            ValueType = JValueType;
            Value = JValue;
        }

        public T Get<T>()
        {
            if (Value is T TValue)
            {
                return TValue;
            }

            return default(T);
        }

        public JsonValue this[int Index] => ValueType == JsonValueType.Array ? (Value as JsonArray)[Index] : null;

        public JsonValue this[string Key] => ValueType == JsonValueType.Object ? (Value as JsonObject)[Key] : null;

        public static explicit operator bool(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Boolean)
            {
                return (bool)Value.Value;
            }

            throw new Exception("explicit operator(bool) error");
        }

        public static explicit operator string(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.String)
            {
                return (string)Value.Value;
            }

            throw new Exception("explicit operator(string) error");
        }

        public static explicit operator short(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (short)Convert.ChangeType(Value.Value, typeof(short));
            }

            throw new Exception("explicit operator(short) error");
        }

        public static explicit operator ushort(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (ushort)Convert.ChangeType(Value.Value, typeof(ushort));
            }

            throw new Exception("explicit operator(ushort) error");
        }

        public static explicit operator int(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (int)Convert.ChangeType(Value.Value, typeof(int));
            }

            throw new Exception("explicit operator(int) error");
        }

        public static explicit operator uint(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (uint)Convert.ChangeType(Value.Value, typeof(uint));
            }

            throw new Exception("explicit operator(uint) error");
        }

        public static explicit operator long(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (long)Convert.ChangeType(Value.Value, typeof(long));
            }

            throw new Exception("explicit operator(long) error");
        }

        public static explicit operator ulong(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (ulong)Convert.ChangeType(Value.Value, typeof(ulong));
            }

            throw new Exception("explicit operator(ulong) error");
        }

        public static explicit operator float(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (float)Convert.ChangeType(Value.Value, typeof(float));
                return (float)Value.Value;
            }

            throw new Exception("explicit operator(float) error");
        }

        public static explicit operator double(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Numeric)
            {
                return (double)Convert.ChangeType(Value.Value, typeof(double));
            }

            throw new Exception("explicit operator(double) error");
        }

        public static explicit operator JsonArray(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Array)
            {
                return Value.Value as JsonArray;
            }

            throw new Exception("explicit operator(JsonArray) error");
        }

        public static explicit operator JsonObject(JsonValue Value)
        {
            if (Value.ValueType == JsonValueType.Object)
            {
                return Value.Value as JsonObject;
            }

            throw new Exception("explicit operator(JsonObject) error");
        }
    }
}