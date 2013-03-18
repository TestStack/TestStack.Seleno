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

        public IWebElement Element(By findExpression, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElement(findExpression), waitInSeconds);
        }

        public IWebElement Element(Locators.By.jQueryBy jQueryFindExpression, int waitInSeconds = 20)
        {
            return Browser.ElementWithWait(d => d.FindElementByjQuery(jQueryFindExpression), waitInSeconds);
        }

        public IWebElement OptionalElement(By findExpression)
        {
            IWebElement result = null;
            try
            {
                result = Browser.FindElement(findExpression);
            }
            catch (NoSuchElementException)
            { }

            return result;
        }


        public IWebElement OptionalElement(Locators.By.jQueryBy jQueryFindExpression)
        {
            IWebElement result = null;
            try
            {
                result = Browser.FindElementByjQuery(jQueryFindExpression);
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