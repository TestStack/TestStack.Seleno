using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class ScriptExecutor : IScriptExecutor
    {
        protected RemoteWebDriver Browser;
        private readonly IElementFinder _finder;
        private readonly ICamera _camera;

        internal ScriptExecutor(RemoteWebDriver browser, IElementFinder finder, ICamera camera)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (finder == null) throw new ArgumentNullException("finder");
            if (camera == null) throw new ArgumentNullException("camera");
            Browser = browser;
            _finder = finder;
            _camera = camera;
        }

        public IWebElement ActionOnLocator(By findElement, Action<IWebElement> action)
        {
            try
            {
                var element = Browser.FindElement(findElement);
                action(element);
                return element;
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        public TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func)
        {
            try
            {
                var element = Browser.FindElement(findElement);
                return func(element);
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        public IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
        {
            try
            {
                var element = _finder.ElementWithWait(findElement, waitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        public object ScriptAndReturn(string javascriptToBeExecuted, Type returnType, IJavaScriptExecutor javaScriptExecutor = null)
        {
            javaScriptExecutor = javaScriptExecutor ?? Browser;

            var untypedValue = javaScriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
            var result = untypedValue.TryConvertTo(returnType, null);

            return result;
        }
    }
}