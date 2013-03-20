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

        // todo: Are these ActionOnLocator methods something that should be in ScriptExecutor or should ScriptExecutor be renamed?
        // todo: Should we add jQuery By method overloads?

        public IWebElement ActionOnLocator(By findElement, Action<IWebElement> action, int maxWaitInSeconds = 5)
        {
            try
            {
                var element = _finder.Element(findElement, maxWaitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                _camera.TakeScreenshot();
                throw;
            }
        }

        public TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func, int maxWaitInSeconds = 5)
        {
            try
            {
                var element = _finder.Element(findElement, maxWaitInSeconds);
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