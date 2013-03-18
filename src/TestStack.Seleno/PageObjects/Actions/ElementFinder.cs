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
            return Browser.ElementWithWait(d => d.FindElement(findElement), waitInSeconds);
        }

        public IWebElement Element(Locators.By.jQueryBy @by, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElementByjQuery(by), waitInSeconds);
        }

        public IWebElement OptionalElement(By by)
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


        public IWebElement OptionalElement(Locators.By.jQueryBy by)
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

        public IWebElement TryFindElementByjQuery(Locators.By.jQueryBy @by)
        {
            return OptionalElement(by);
        }
    }
}