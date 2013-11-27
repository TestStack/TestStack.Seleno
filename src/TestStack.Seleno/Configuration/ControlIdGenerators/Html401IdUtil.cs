using System.Text;

namespace TestStack.Seleno.Configuration.ControlIdGenerators
{
    /// <summary>
    /// Copied from System.Web.Mvc.TagBuilder
    /// </summary>
    internal static class Html401IdUtil
    {
        public static string CreateSanitizedId(string originalId)
        {
            if (string.IsNullOrEmpty(originalId))
                return null;
            char c1 = originalId[0];
            if (!IsLetter(c1))
                return null;
            var stringBuilder = new StringBuilder(originalId.Length);
            stringBuilder.Append(c1);
            for (var index = 1; index < originalId.Length; ++index)
            {
                char c2 = originalId[index];
                if (IsValidIdCharacter(c2))
                    stringBuilder.Append(c2);
                else
                    stringBuilder.Append("_");
            }
            return stringBuilder.ToString();
        }

        private static bool IsAllowableSpecialCharacter(char c)
        {
            switch (c)
            {
                case '-':
                case ':':
                case '_':
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsDigit(char c)
        {
            if (48 <= c)
                return c <= 57;
            return false;
        }

        private static bool IsLetter(char c)
        {
            if (65 <= c && c <= 90)
                return true;
            if (97 <= c)
                return c <= 122;
            return false;
        }

        private static bool IsValidIdCharacter(char c)
        {
            if (!IsLetter(c) && !IsDigit(c))
                return IsAllowableSpecialCharacter(c);
            return true;
        }
    }
}
