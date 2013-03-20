using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.Extensions
{
    /// <summary>
    /// Extension methods against an <see cref="IWebElement"/>.
    /// </summary>
    public static class WebElementExtensions
    {
        /// <summary>
        /// Returns the value for the given HTML attribute on the <see cref="IWebElement"/> and type casts it the the requested type.
        /// </summary>
        /// <typeparam name="TReturn">The type to cast the value to</typeparam>
        /// <param name="element">The element to return the attribute value from</param>
        /// <param name="attributeName">The name of the attribute to return the value for</param>
        /// <returns>The type-casted value</returns>
        public static TReturn GetAttributeAs<TReturn>(this IWebElement element, string attributeName)
        {
            var attributeValue = string.Empty;

            if (element != null)
            {
                attributeValue = element.GetAttribute(attributeName) ?? string.Empty;
            }

            return attributeValue.TryConvertTo(default(TReturn));
        }

        /// <summary>
        /// Returns the value for the "value" HTML attribute on the <see cref="IWebElement"/> and type casts it the the requested type.
        /// </summary>
        /// <typeparam name="TReturn">The type to cast the value to</typeparam>
        /// <param name="element">The element to return the value attribute value from</param>
        /// <returns>The type-casted value attribute value</returns>
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