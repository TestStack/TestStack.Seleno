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

            object untypedValue = javaScriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
            object result = untypedValue.TryConvertTo(returnType, null);

            return result;
        }

        public void ExecuteScript(string javascriptToBeExecuted, IJavaScriptExecutor javaScriptExecutor = null)
        {
            javaScriptExecutor = javaScriptExecutor ?? Browser;
            javaScriptExecutor.ExecuteScript(javascriptToBeExecuted);
        }
    }
}