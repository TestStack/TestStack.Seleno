using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Specifications.Assertions
{
    public interface IElementAssert
    {
        IElementFinder Find { get; }
        IElementAssert DoNotExist(By selector, string message = null, TimeSpan maxWait = default(TimeSpan));
        IElementAssert DoNotExist(PageObjects.Locators.By.jQueryBy selector, string message = null, TimeSpan maxWait = default(TimeSpan));
        IElementAssert Exist(By selector, string message = null, TimeSpan maxWait = default(TimeSpan));
        IElementAssert Exist(PageObjects.Locators.By.jQueryBy selector, string message = null, TimeSpan maxWait = default(TimeSpan));
        IElementAssert ConformTo(By selector, Action<IEnumerable<IWebElement>> assertion, TimeSpan maxWait = default(TimeSpan));
        IElementAssert ConformTo(PageObjects.Locators.By.jQueryBy selector, Action<IEnumerable<IWebElement>> assertion, TimeSpan maxWait = default(TimeSpan));
    }
}