using System.Collections.Generic;
using System.Text;

namespace LiteJson.Common
{
    internal static class Logger
    {
        private static List<string> Msgs_ = new List<string>();

        public static void Add(int Line, int Index, string Msg)
        {
            Msgs_.Add($"[{Line}:{Index}] {Msg}");
        }

        public static void Clear()
        {
            Msgs_.Clear();
        }

        public static string Get()
        {
            var Result = new StringBuilder();

            foreach (var Msg in Msgs_)
            {
                Result.Append(Msg);
            }

            return Result.ToString();
        }
    }
}