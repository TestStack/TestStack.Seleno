using System;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class ScriptExecutor : IScriptExecutor
    {
        private readonly IJavaScriptExecutor _javaScriptExecutor;
        private readonly IElementFinder _finder;
        private readonly ICamera _camera;

        public ScriptExecutor(IJavaScriptExecutor javaScriptExecutor, IElementFinder finder, ICamera camera)
        {
            if (javaScriptExecutor == null) throw new ArgumentNullException("javaScriptExecutor");
            if (finder == null) throw new ArgumentNullException("finder");
            if (camera == null) throw new ArgumentNullException("camera");
            
            _javaScriptExecutor = javaScriptExecutor;
            _finder = finder;
            _camera = camera;
        }

        public IWebElement ActionOnLocator(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
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

        public TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func, int waitInSeconds = 20)
        {
            try
            {
                var element = _finder.ElementWithWait(findElement, waitInSeconds);
                return func(element);
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
    }
}