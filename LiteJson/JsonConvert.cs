using System;
using System.Reflection;
using System.Text;
using LiteJson.Common;
using LiteJson.Stream;
using LiteJson.Syntax;
using LiteJson.Analyzer;

namespace LiteJson
{
    public static class JsonConvert
    {
        public static string GetErrorMsg()
        {
            return Logger.Get();
        }

        public static string SerializeObject<T>(T Obj)
        {
            Logger.Clear();
            var JsonNode = ParseNodeFromObject(typeof(T), Obj);
            var Code = new CodeAnalyzer(JsonNode);

            if (!Code.Analyzing())
            {
                return string.Empty;
            }

            return Code.GetJsonString();
        }

        private static SyntaxObjectNode ParseNodeFromObject(Type ObjType, object Obj)
        {
            var Node = new SyntaxObjectNode();
            var Fileds = ObjType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var Filed in Fileds)
            {
                var FiledValue = Filed.GetValue(Obj);
                var FiledNode = ParseNodeFromValue(Filed.FieldType, FiledValue);
                Node.AddChild(Filed.Name, FiledNode);
            }

            return Node;
        }

        private static SyntaxNode ParseNodeFromValue(Type ValueType, object Obj)
        {
            if (ValueType == typeof(bool))
            {
                return new SyntaxLiteralBooleanNode(Obj.ToString().ToLower());
            }

            if (ValueType == typeof(string))
            {
                return new SyntaxLiteralStringNode(Obj.ToString());
            }

            if (ValueType.IsPrimitive && ValueType.IsValueType)
            {
                return new SyntaxLiteralNumericNode(Obj.ToString());
            }

            if (ValueType.IsArray)
            {
                var ArrayNode = new SyntaxArrayNode();
                var ElementType = ValueType.GetElementType();
                var Length = (int) (ValueType.InvokeMember("get_Length", BindingFlags.InvokeMethod, null, Obj, null));

                for (var Index = 0; Index < Length; ++Index)
                {
                    var ElementValue = ValueType.GetMethod("GetValue", new[] {typeof(int)}).Invoke(Obj, new object[] {Index});
                    var ElementNode = ParseNodeFromValue(ElementType, ElementValue);
                    ArrayNode.AddValue(ElementNode);
                }

                return ArrayNode;
            }

            if (ValueType.IsClass || (ValueType.IsValueType && !ValueType.IsPrimitive))
            {
                return ParseNodeFromObject(ValueType, Obj);
            }

            return new SyntaxLiteralNullNode("null");
        }

        public static T DeserializeObject<T>(string Json) where T : class, new()
        {
            Logger.Clear();
            var CharStream = new CharacterStream(Json);
            var Lexer = new LexicalAnalyzer(CharStream);
            if (!Lexer.Analyzing())
            {
                return null;
            }

            var Parser = new SyntaxAnalyzer(Lexer.GetTokenStream());
            if (!Parser.Analyzing())
            {
                return null;
            }

            var JsonNode = Parser.GetJsonNode() as SyntaxObjectNode;
            return ParseObjectFromNode(typeof(T), JsonNode) as T;
        }

        private static object ParseObjectFromNode(Type ObjType, SyntaxObjectNode Node)
        {
            var Obj = Activator.CreateInstance(ObjType);
            var Keys = Node.GetKeys();

            foreach (var Key in Keys)
            {
                var Info = ObjType.GetField(Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (Info == null)
                {
                    continue;
                }

                var ChildValue = ParseValueFromNode(Info.FieldType, Node.GetValue(Key));
                Info.SetValue(Obj, ChildValue);
            }

            return Obj;
        }

        private static object ParseValueFromNode(Type ValueType, SyntaxNode Node)
        {
            if (Node.GetType() == SyntaxNodeType.Literal)
            {
                if (Node is SyntaxLiteralNullNode)
                {
                    return null;
                }

                if (Node is SyntaxLiteralBooleanNode && ValueType == typeof(bool))
                {
                    return (Node as SyntaxLiteralBooleanNode).GetValue();
                }

                if (Node is SyntaxLiteralStringNode && ValueType == typeof(string))
                {
                    return (Node as SyntaxLiteralStringNode).GetValue();
                }

                if (Node is SyntaxLiteralNumericNode && ValueType.IsPrimitive && ValueType.IsValueType)
                {
                    var FiledValue = (Node as SyntaxLiteralNumericNode).GetValue();
                    return Convert.ChangeType(FiledValue, ValueType);
                }
            }
            else if (Node.GetType() == SyntaxNodeType.Array && ValueType.IsArray)
            {
                var ChildType = ValueType.GetElementType();
                var ArrayValues = (Node as SyntaxArrayNode).GetValues();
                var ArrayObject = new object();
                ArrayObject = ValueType.InvokeMember("Set", BindingFlags.CreateInstance, null, ArrayObject, new object[] {ArrayValues.Length});

                for (var Index = 0; Index < ArrayValues.Length; ++Index)
                {
                    var ChildValue = ParseValueFromNode(ChildType, ArrayValues[Index]);
                    ValueType.GetMethod("SetValue", new[] {typeof(object), typeof(int)})
                        .Invoke(ArrayObject, new[] {ChildValue, Index});
                }

                return ArrayObject;
            }
            else if (Node.GetType() == SyntaxNodeType.Object && (ValueType.IsClass || (ValueType.IsValueType && !ValueType.IsPrimitive)))
            {
                return ParseObjectFromNode(ValueType, Node as SyntaxObjectNode);
            }

            return null;
        }

        public static JsonObject DeserializeJsonObject(string Json)
        {
            Logger.Clear();
            var CharStream = new CharacterStream(Json);
            var Lexer = new LexicalAnalyzer(CharStream);
            if (!Lexer.Analyzing())
            {
                return null;
            }

            var Parser = new SyntaxAnalyzer(Lexer.GetTokenStream());
            if (!Parser.Analyzing())
            {
                return null;
            }

            var JsonNode = Parser.GetJsonNode() as SyntaxObjectNode;
            return ParseJsonObjectFromNode(JsonNode);
        }

        private static JsonObject ParseJsonObjectFromNode(SyntaxObjectNode Node)
        {
            var Keys = Node.GetKeys();
            var JsonObj = new JsonObject();

            foreach (var Key in Keys)
            {
                var Value = ParseJsonValueFromNode(Node.GetValue(Key));
                JsonObj.SetValue(Key, Value);
            }

            return JsonObj;
        }

        private static JsonValue ParseJsonValueFromNode(SyntaxNode Node)
        {
            var NodeType = Node.GetType();

            switch (NodeType)
            {
                case SyntaxNodeType.Literal:
                    switch (Node)
                    {
                        case SyntaxLiteralNullNode LiteralNode:
                            return null;
                        case SyntaxLiteralBooleanNode LiteralNode:
                            return new JsonValue(JsonValueType.Boolean, LiteralNode.GetValue());
                        case SyntaxLiteralStringNode LiteralNode:
                            return new JsonValue(JsonValueType.String, LiteralNode.GetValue());
                        case SyntaxLiteralNumericNode LiteralNode:
                            return new JsonValue(JsonValueType.Numeric, LiteralNode.GetValue());
                        default:
                            break;
                    }
                    break;
                case SyntaxNodeType.Array:
                    var JsonArr = new JsonArray();
                    var ArrayValues = (Node as SyntaxArrayNode).GetValues();

                    foreach (var ElementNode in ArrayValues)
                    {
                        var ElementValue = ParseJsonValueFromNode(ElementNode);
                        JsonArr.Add(ElementValue);
                    }

                    return new JsonValue(JsonValueType.Array, JsonArr);
                case SyntaxNodeType.Object:
                    return new JsonValue(JsonValueType.Object, ParseJsonObjectFromNode(Node as SyntaxObjectNode));
                default:
                    break;
            }
            
            return null;
        }
    }
}