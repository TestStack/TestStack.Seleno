using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TestStack.Seleno.PageObjects.Locators;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class ElementFinder : IElementFinder
    {
        protected IWebDriver Browser;

        internal ElementFinder(IWebDriver browser)
        {
            if(browser == null)
                throw new ArgumentNullException("browser");
            Browser = browser;
        }

        public IWebElement ElementWithWait(By findElement, int waitInSeconds = 20)
        {
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(waitInSeconds));
            return wait.Until(d => d.FindElement(findElement));
        }

        public IWebElement TryFindElement(By by)
        {
            IWebElement result = null;
            try
            {
                result = Browser.FindElement(by);
            }
            catch (NoSuchElementException)
            {
            }

            return result;
        }

        public IWebElement TryFindElementByjQuery(Locators.By.jQueryBy @by)
        {
            IWebElement result = null;
            try
            {
                result = Browser.FindElementByjQuery(@by);
            }
            catch (NoSuchElementException)
            {
            }

            return result;
        }
    }
}