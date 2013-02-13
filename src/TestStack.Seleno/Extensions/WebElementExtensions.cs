using System.ComponentModel;
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
    }
}