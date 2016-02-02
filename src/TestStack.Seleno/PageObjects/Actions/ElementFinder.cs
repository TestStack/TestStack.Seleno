using System;
using System.Collections.Generic;
using System.Linq;
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
                throw new ArgumentNullException(nameof(browser));
            Browser = browser;
        }

        public IWebElement Element(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return Browser.ElementWithWait(d => d.FindElement(findExpression), maxWait);
        }

        public IWebElement Element(Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            return Browser.ElementWithWait(d => d.FindElementByjQuery(jQueryFindExpression), maxWait);
        }

        public IEnumerable<IWebElement> Elements(By findExpression, TimeSpan maxWait = new TimeSpan())
        {
            var atLeastOneElement = OptionalElement(findExpression, maxWait);
            if (atLeastOneElement == null)
                return Enumerable.Empty<IWebElement>();

            return Browser.FindElements(findExpression);
        }

        public IEnumerable<IWebElement> Elements(Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = new TimeSpan())
        {
            var atLeastOneElement = OptionalElement(jQueryFindExpression, maxWait);
            if (atLeastOneElement == null)
                return Enumerable.Empty<IWebElement>();

            return Browser.FindElements(jQueryFindExpression);
        }

        public IWebElement OptionalElement(By findExpression, TimeSpan maxWait = default(TimeSpan))
        {
            try
            {
                return Element(findExpression, maxWait);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }


        public IWebElement OptionalElement(Locators.By.jQueryBy jQueryFindExpression, TimeSpan maxWait = default(TimeSpan))
        {
            try
            {
                return Element(jQueryFindExpression, maxWait);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)
        {
            throw new NotImplementedException("Obsolete");
        }

        public IWebElement TryFindElement(By @by)
        {
            throw new NotImplementedException("Obsolete");
        }

        public IWebElement TryFindElementByjQuery(Locators.By.jQueryBy @by)
        {
            throw new NotImplementedException("Obsolete");
        }
    }
}