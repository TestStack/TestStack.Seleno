using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IScriptExecutor
    {
        IWebElement ActionOnLocator(By findElement, Action<IWebElement> action);
        TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func);
        IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20);
        object ScriptAndReturn(string javascriptToBeExecuted, Type returnType, params object[] arguments);
        TReturn ScriptAndReturn<TReturn>(string javascriptToBeExecuted, params object[] arguments);
        void ExecuteScript(string javascriptToBeExecuted, params object[] arguments);
    }
}