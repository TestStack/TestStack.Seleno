using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IElementFinder
    {
        IWebElement ElementWithWait(By findElement, int waitInSeconds = 20);
        IWebElement ElementWithWait(Locators.By.jQueryBy by, int waitInSeconds = 20);
        IWebElement TryFindElement(By by, int waitInSeconds = 0);
        IWebElement TryFindElement(Locators.By.jQueryBy by, int waitInSeconds = 0);
        [Obsolete("Use TryFindElement instead")]
        IWebElement TryFindElementByjQuery(Locators.By.jQueryBy by);
    }
}