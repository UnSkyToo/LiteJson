using System.Collections.Generic;

namespace LiteJson.Common
{
    internal static class LanguageDescriptor
    {
        public const string JsonObjectBegin       = "{";
        public const string JsonObjectEnd         = "}";
        public const string JsonArrayBegin        = "[";
        public const string JsonArrayEnd          = "]";
        public const string JsonValueDelimiter    = ",";
        public const string JsonDefineDelimiter   = ":";

        private enum CharaterType
        {
            Delimiter = 0,
            Whitespace = 1,
        }

        private static readonly Dictionary<char, CharaterType> Charaters_ = new Dictionary<char, CharaterType>
        {
            // Delimiter
            {JsonObjectBegin[0], CharaterType.Delimiter},
            {JsonObjectEnd[0], CharaterType.Delimiter},
            {JsonArrayBegin[0], CharaterType.Delimiter},
            {JsonArrayEnd[0], CharaterType.Delimiter},
            {JsonValueDelimiter[0], CharaterType.Delimiter},
            {JsonDefineDelimiter[0], CharaterType.Delimiter},
            // Whitespace
            {' ', CharaterType.Whitespace},
            {'\t', CharaterType.Whitespace},
            {'\r', CharaterType.Whitespace}
        };
        
        public static bool IsDelimiterChar(char Value)
        {
            return Charaters_.ContainsKey(Value) && Charaters_[Value] == CharaterType.Delimiter;
        }

        public static bool IsWhitespaceChar(char Value)
        {
            return Charaters_.ContainsKey(Value) && Charaters_[Value] == CharaterType.Whitespace;
        }

        public static bool IsEndOfLineChar(char Value)
        {
            return Value == '\n';
        }

        public static bool IsDigitChar(char Value)
        {
            return Value >= '0' && Value <= '9';
        }

        public static bool IsIdentityChar(char Value)
        {
            return (Value >= 'a' && Value <= 'z') ||
                   (Value >= 'A' && Value <= 'Z') ||
                   Value == '_';
        }

        public static bool IsQuoteChar(char Value)
        {
            return Value == '"' || Value == '\'';
        }

        public static bool IsEscapeChar(char Value)
        {
            return Value == '\\';
        }

        public static char EscapeChar(char Value)
        {
            switch (Value)
            {
                case 'n':
                    return '\n';
                case 't':
                    return '\t';
                case 'b':
                    return '\b';
                case 'r':
                    return '\r';
                case 'f':
                    return '\f';
                default:
                    return Value;
            }
        }

        public static bool IsPointChar(char Value)
        {
            return Value == '.';
        }

        public static bool IsBooleanString(string Value)
        {
            return (Value == "true" || Value == "false");
        }

        public static bool IsNullString(string Value)
        {
            return Value == "null";
        }
    }
}