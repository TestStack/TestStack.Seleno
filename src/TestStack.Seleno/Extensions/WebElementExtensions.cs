using System.ComponentModel;
using OpenQA.Selenium;

namespace TestStack.Seleno.Extensions
{
    public static class WebElementExtensions
    {
        public static TReturn GetAttributeAs<TReturn>(this IWebElement element, string attributeName)
        {
            var attributeValue = element.GetAttribute(attributeName) ?? string.Empty;

            return (TReturn)TypeDescriptor.GetConverter(typeof(TReturn)).ConvertFromString(attributeValue);
        }
    }
}