using System;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class Executor : IExecutor
    {
        private readonly IJavaScriptExecutor _javaScriptExecutor;
        private readonly IElementFinder _finder;
        private readonly ICamera _camera;

        public Executor(IJavaScriptExecutor javaScriptExecutor, IElementFinder finder, ICamera camera)
        {
            if (javaScriptExecutor == null) throw new ArgumentNullException("javaScriptExecutor");
            if (finder == null) throw new ArgumentNullException("finder");
            if (camera == null) throw new ArgumentNullException("camera");
            
            _javaScriptExecutor = javaScriptExecutor;
            _finder = finder;
            _camera = camera;
        }

        public IWebElement ActionOnLocator(By findExpression, Action<IWebElement> action, int maxWaitInSeconds = 5)
        {
            try
            {
                var element = _finder.Element(findExpression, maxWaitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        // todo: unit/integration test this
        public IWebElement ActionOnLocator(Locators.By.jQueryBy jQueryFindExpression, Action<IWebElement> action, int maxWaitInSeconds = 5)
        {
            try
            {
                var element = _finder.Element(jQueryFindExpression, maxWaitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        public object ScriptAndReturn(string javascriptToBeExecuted, Type returnType)
        {
            var untypedValue = _javaScriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
            var result = untypedValue.TryConvertTo(returnType, null);

            return result;
        }

        public TReturn ScriptAndReturn<TReturn>(string javascriptToBeExecuted)
        {
            return (TReturn) ScriptAndReturn(javascriptToBeExecuted, typeof (TReturn));
        }

        public void ExecuteScript(string javascriptToBeExecuted)
        {
            _javaScriptExecutor.ExecuteScript(javascriptToBeExecuted);
        }

        public TResult ActionOnLocator<TResult>(By findExpression, Func<IWebElement, TResult> func, int maxWaitInSeconds = 5)
        {
            throw new NotImplementedException();
        }

        public IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
        {
            throw new NotImplementedException();
        }
    }
}