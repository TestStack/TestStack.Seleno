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

        public IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElement(findElement));
        }

        public IWebElement ElementWithWait(Locators.By.jQueryBy @by, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElementByjQuery(by));
        }

        public IWebElement TryFindElement(By by)
        {
            IWebElement result = null;
            try
            {
                result =  Browser.FindElement(by);
            }
            catch (NoSuchElementException)
            { }

            return result;
        }


        public IWebElement TryFindElement(Locators.By.jQueryBy by)
        {
            IWebElement result = null;
            try
            {
                result = Browser.FindElementByjQuery(by);
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