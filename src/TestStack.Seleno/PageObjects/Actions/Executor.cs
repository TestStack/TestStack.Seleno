using System;
using System.Threading;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class Executor : IExecutor
    {
        private readonly IJavaScriptExecutor _javaScriptExecutor;
        private readonly IElementFinder _finder;
        private readonly ICamera _camera;

        public Executor(IJavaScriptExecutor javaScriptExecutor, IElementFinder finder, ICamera camera)
        {
            if (javaScriptExecutor == null) throw new ArgumentNullException(nameof(javaScriptExecutor));
            if (finder == null) throw new ArgumentNullException(nameof(finder));
            if (camera == null) throw new ArgumentNullException(nameof(camera));
            
            _javaScriptExecutor = javaScriptExecutor;
            _finder = finder;
            _camera = camera;
        }

        public IWebElement ActionOnLocator(By findExpression, Action<IWebElement> action, TimeSpan maxWait = default(TimeSpan))
        {
            var element = _finder.Element(findExpression, maxWait);
            action(element);
            return element;
        }

        // todo: unit/integration test this
        public IWebElement ActionOnLocator(Locators.By.jQueryBy jQueryFindExpression, Action<IWebElement> action, TimeSpan maxWait = default(TimeSpan))
        {
            var element = _finder.Element(jQueryFindExpression, maxWait);
            action(element);
            return element;
        }

        public void PredicateScriptAndWaitToComplete(string predicateScriptToBeExecuted, TimeSpan maxWait = default(TimeSpan))
        {
            if (maxWait == default(TimeSpan)) maxWait = TimeSpan.FromSeconds(5);
            var end = SystemTime.Now() + maxWait;
            
            var isComplete = false;

            while (SystemTime.Now() < end)
            {
                isComplete = ScriptAndReturn<bool>(predicateScriptToBeExecuted);
                
                if (isComplete)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            if (!isComplete)
            {
                throw new TimeoutException(
                    $"The predicate script took longer than {maxWait.TotalSeconds} seconds to verify statement");
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

        public void Script(string javascriptToBeExecuted)
        {
            _javaScriptExecutor.ExecuteScript(javascriptToBeExecuted);
        }

        [Obsolete]
        public TResult ActionOnLocator<TResult>(By findExpression, Func<IWebElement, TResult> func, int maxWaitInSeconds = 5)
        {
            throw new NotImplementedException("Obsolete");
        }

        [Obsolete]
        public IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
        {
            throw new NotImplementedException("Obsolete");
        }
    }
}