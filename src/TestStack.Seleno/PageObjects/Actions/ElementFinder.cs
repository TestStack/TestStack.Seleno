using System;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Locators;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class ElementFinder : IElementFinder
    {
        protected IWebDriver Browser;

        public ElementFinder(IWebDriver browser)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");
            Browser = browser;
        }

        public IWebElement Element(By findElement, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElement(findElement));
        }

        public IWebElement Element(Locators.By.jQueryBy @by, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElementByjQuery(by));
        }

        public IWebElement TryFindElement(By by, int waitInSeconds = 0)
        {
            IWebElement result = null;
            try
            {
                result = Element(by, waitInSeconds);
            }
            catch (NoSuchElementException)
            { }

            return result;
        }


        public IWebElement TryFindElement(Locators.By.jQueryBy by, int waitInSeconds = 0)
        {
            IWebElement result = null;
            try
            {
                result = Element(by, waitInSeconds);
            }
            catch (NoSuchElementException)
            { }

            return result;
        }

        [Obsolete("Use TryFindElement instead")]
        public IWebElement TryFindElementByjQuery(Locators.By.jQueryBy @by)
        {
            return TryFindElement(by);
        }
    }
}