using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.Extensions
{
    public static class WebElementExtensions
    {
        public static TReturn GetAttributeAs<TReturn>(this IWebElement element, string attributeName)
        {
            var attributeValue = string.Empty;

            if (element != null)
            {
                attributeValue = element.GetAttribute(attributeName) ?? string.Empty;
            }

            return attributeValue.TryConvertTo(default(TReturn));
        }

        public static TReturn GetControlValueAs<TReturn>(this IWebElement element)
        {
            return element.GetAttributeAs<TReturn>("value");
        }

        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static void ClearAndSendKeys(this IWebElement element, string value, bool clearFirst = true)
        {
            throw new NotImplementedException();
        }
        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static void SetAttribute(this IWebElement element, string attributeName, string value)
        {
            throw new NotImplementedException();
        }
        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static void ReplaceInputValueWith(this IWebDriver driver, string inputSelector, string value)
        {
            throw new NotImplementedException();
        }
        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static T GetAttributeAsType<T>(this IWebElement element, string attributeName)
        {
            throw new NotImplementedException();
        }
        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static T GetValueFromTextBox<T>(this IWebElement element)
        {
            throw new NotImplementedException();
        }
        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        public static T TextAsType<T>(this IWebElement element)
        {
            throw new NotImplementedException();
        }
    }
}