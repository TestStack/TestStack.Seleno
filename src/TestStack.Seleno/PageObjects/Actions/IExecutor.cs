using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IExecutor
    {
        IWebElement ActionOnLocator(By findExpression, Action<IWebElement> action, int maxWaitInSeconds = 5);

        TResult ActionOnLocator<TResult>(By findExpression, Func<IWebElement, TResult> func, int maxWaitInSeconds = 5);

        object ScriptAndReturn(string javascriptToBeExecuted, Type returnType);

        TReturn ScriptAndReturn<TReturn>(string javascriptToBeExecuted);

        void ExecuteScript(string javascriptToBeExecuted);

        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: Use ActionOnLocator. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20);
    }
}