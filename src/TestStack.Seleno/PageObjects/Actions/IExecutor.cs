using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    /// <summary>Obsolete</summary>
    [Obsolete("Obsolete: Use IExecutor. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
    public interface IScriptExecutor {}

    /// <summary>
    /// Executes JavaScript on the page and actions on elements in the page.
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Locate an element using a Selenium Web Driver expression and performs an action on the element if found.
        /// Throws an exception if the element isn't found.
        /// </summary>
        /// <example>
        /// Execute.ActionOnLocator(By.Id("helloworld"), e => e.ClearAndSendKeys("xyz"))
        /// </example>
        /// <param name="findExpression">Selenium Web Driver expression to find an element</param>
        /// <param name="action">The action to perform when the element is found</param>
        /// <param name="maxWait">Maximum amount of time to wait for the element to become available</param>
        /// <exception cref="NoSuchElementException">When the element isn't found</exception>
        /// <returns>The <see cref="IWebElement"/> representing the found element</returns>
        IWebElement ActionOnLocator(By findExpression, Action<IWebElement> action, TimeSpan maxWait = default(TimeSpan));

        /// <summary>
        /// Locate an element using a jQuery expression and performs an action on the element if found.
        /// Throws an exception if the element isn't found.
        /// </summary>
        /// <example>
        /// Execute.ActionOnLocator(By.jQuery("#helloworld"), e => e.ClearAndSendKeys("xyz"))
        /// </example>
        /// <param name="jQueryFindExpression">jQuery expression to find an element</param>
        /// <param name="action">The action to perform when the element is found</param>
        /// <param name="maxWait">Maximum amount of time to wait for the element to become available</param>
        /// <exception cref="NoSuchElementException">When the element isn't found</exception>
        /// <returns>The <see cref="IWebElement"/> representing the found element</returns>
        IWebElement ActionOnLocator(Locators.By.jQueryBy jQueryFindExpression, Action<IWebElement> action, TimeSpan maxWait = default(TimeSpan));

        /// <summary>
        /// Executes some JavaScript that returns a boolean and waits until its returned value is true
        /// </summary>
        /// <param name="predicateScriptToBeExecuted">The predicate javaScript to execute</param>
        /// <param name="maxWait">Maximum amount of time to wait for predicate Javascript to return true (default is 5 seconds)</param>
        /// <exception cref="TimeoutException">When the executed JavaScript took more than maxWait to execute</exception>
        void PredicateScriptAndWaitToComplete(string predicateScriptToBeExecuted, TimeSpan maxWait = default(TimeSpan));
        
        /// <summary>
        /// Executes some JavaScript and returns the return value type-casted to the given type.
        /// </summary>
        /// <param name="javascriptToBeExecuted">The JavaScript to execute</param>
        /// <param name="returnType">The type to cast the return value to</param>
        /// <returns>The type-casted value as returned from the JavaScript execution</returns>
        object ScriptAndReturn(string javascriptToBeExecuted, Type returnType);

        /// <summary>
        /// Executes some JavaScript and returns the return value type-casted to the given type.
        /// </summary>
        /// <typeparam name="TReturn">The type to cast the return value to</typeparam>
        /// <param name="javascriptToBeExecuted">The JavaScript to execute</param>
        /// <returns>The type-casted value as returned from the JavaScript execution</returns>
        TReturn ScriptAndReturn<TReturn>(string javascriptToBeExecuted);

        /// <summary>
        /// Executes the given JavaScript.
        /// </summary>
        /// <param name="javascriptToBeExecuted">The JavaScript to execute</param>
        void Script(string javascriptToBeExecuted);

        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: Use Find.Element() and map it to the value you need. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        TResult ActionOnLocator<TResult>(By findExpression, Func<IWebElement, TResult> func, int maxWaitInSeconds = 5);

        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: Use ActionOnLocator. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20);
    }
}