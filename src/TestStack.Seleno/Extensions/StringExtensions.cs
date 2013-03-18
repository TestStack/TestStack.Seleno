namespace TestStack.Seleno.Extensions
{
    internal static class StringExtensions
    {
        internal static string ToJavaScriptString(this string str)
        {
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }
    }
}
