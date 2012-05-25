using System;
using System.ComponentModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Seleno.Extensions
{
    public static class WebElementExtensions
    {
        public static void ClearAndSendKeys(this IWebElement element, string value, bool clearFirst = true)
        {
            if (clearFirst) element.Clear();
            element.SendKeys(value);
        }

        public static void SetAttribute(this IWebElement element, string attributeName, string value)
        {
            IWrapsDriver wrappedElement = element as IWrapsDriver;
            if (wrappedElement == null)
                throw new ArgumentException("element", "Element must wrap a web driver");

            IWebDriver driver = wrappedElement.WrappedDriver;
            IJavaScriptExecutor javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("element", "Element must wrap a web driver that supports javascript execution");

            javascript.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])", element, attributeName, value);
        }

        public static void ReplaceInputValueWith(this IWebDriver driver, string inputSelector, string value)
        {
            IJavaScriptExecutor javascript = (IJavaScriptExecutor)driver;
            var script = string.Format("$('{0}').val('{1}')", inputSelector, value);
            javascript.ExecuteScript(script);
        }

        public static T GetAttributeAsType<T>(this IWebElement element, string attributeName)
        {
            string value = element.GetAttribute(attributeName) ?? string.Empty;
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }

        public static T GetValueFromTextBox<T>(this IWebElement element)
        {
            return element.GetAttributeAsType<T>("value");
        }

        public static T TextAsType<T>(this IWebElement element)
        {
            string value = element.Text;
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }
    }
}
