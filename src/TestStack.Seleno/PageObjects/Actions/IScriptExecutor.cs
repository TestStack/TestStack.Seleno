using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IScriptExecutor
    {
        IWebElement ActionOnLocator(By findElement, Action<IWebElement> action, int waitInSeconds = 20);
        TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func, int waitInSeconds = 20);
        object ScriptAndReturn(string javascriptToBeExecuted, Type returnType);
        TReturn ScriptAndReturn<TReturn>(string javascriptToBeExecuted);
        void ExecuteScript(string javascriptToBeExecuted);
    }
}