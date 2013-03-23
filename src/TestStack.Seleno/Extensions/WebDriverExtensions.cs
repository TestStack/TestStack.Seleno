using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TestStack.Seleno.PageObjects.Locators;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Extensions
{
    public static class WebDriverExtensions
    {
        public const int DefaultSecondTimeout = 60;

        public static string TitleWithWait(this IWebDriver webDriver)
        {
            return new WebDriverWait(webDriver, TimeSpan.FromSeconds(5))
                .Until(d => new[] { d.Title }
                    .Select(t => string.IsNullOrEmpty(t) ? null : t)
                    .First()
                );
        }

        public static IWebElement ElementWithWait(this IWebDriver driver, Func<IWebDriver, IWebElement> elementIsFound, TimeSpan maxWait)
        {
            try
            {
                if (maxWait == default(TimeSpan))
                    maxWait = TimeSpan.FromSeconds(5);
                var wait = new WebDriverWait(driver, maxWait);
                return wait.Until(elementIsFound);
            }
            catch (WebDriverTimeoutException e)
            {
                throw e.InnerException;
            }
        }

        public static void WaitForSeconds(this IWebDriver driver, int seconds)
        {
            System.Threading.Thread.Sleep(seconds * 1000);
        }

        public static void WaitForMilliseconds(this IWebDriver driver, int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        public static TReturn ExecuteScriptAndReturn<TReturn>(this IWebDriver driver, string javascriptToBeExecuted)
        {
            return (TReturn)(driver as RemoteWebDriver).ExecuteScript("return " + javascriptToBeExecuted);
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)
        {
            return driver.FindElements(by, predicate).First();
        }

        public static IEnumerable<IWebElement> FindElements(this IWebDriver driver, By by, Func<IWebElement, bool> predicate)
        {
            return driver.FindElements(by).Where(predicate);
        }

        public static int CountNumberOfElements(this IWebDriver browser, By by, Func<IWebElement, Boolean> predicate = null)
        {
            return browser.FindElements(by, predicate).Count();
        }

        public static IWebElement WaitForElement(this IWebDriver driver, OpenQA.Selenium.By by,
                                                 Func<IWebElement, bool> predicate = null,
                                                 int seconds = DefaultSecondTimeout)
        {
            return driver.WaitForElements(by, predicate, seconds).First();
        }

        public static IWebElement WaitForElement(this IWebDriver driver, By.jQueryBy by,
                                                 Func<IWebElement, bool> predicate = null,
                                                 int seconds = DefaultSecondTimeout)
        {
            return driver.WaitForElements(by, predicate, seconds).First();
        }

        static IEnumerable<IWebElement> WaitForElements(this IWebDriver driver,
                                                        Func<IWebDriver, IEnumerable<IWebElement>> elementLocators,
                                                        Func<IWebElement, bool> predicate, int seconds)
        {
            IEnumerable<IWebElement> els;
            var retry = 0;
            do
            {
                retry++;
                driver.WaitForMilliseconds(200);

                els = elementLocators(driver);
                if (predicate != null)
                    els = els.Where(predicate);

            } while (els.Count() == 0 && retry < seconds);

            return els;
        }

        public static IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, By.jQueryBy by,
                                                               Func<IWebElement, bool> predicate = null,
                                                               int seconds = DefaultSecondTimeout)
        {
            return driver.WaitForElements(d => d.FindElements(by), predicate, seconds);
        }

        public static IEnumerable<IWebElement> WaitForElements(this IWebDriver driver, OpenQA.Selenium.By by,
                                                               Func<IWebElement, bool> predicate = null,
                                                               int seconds = DefaultSecondTimeout)
        {
            return driver.WaitForElements(d => d.FindElements(by), predicate, seconds);
        }

        public static IJavaScriptExecutor GetJavaScriptExecutor(this IWebDriver driver)
        {
            return driver as IJavaScriptExecutor;
        }

        public static string GetText(this IWebDriver driver)
        {
            return driver.FindElement(By.TagName("body")).Text;
        }

        public static bool HasElement(this IWebDriver driver, OpenQA.Selenium.By by)
        {
            return driver.HasElement(d => d.FindElement(by));
        }

        public static bool HasElement(this IWebDriver driver, By.jQueryBy byJQuery)
        {
            return driver.HasElement(d => d.FindElementByjQuery(byJQuery));
        }

        static bool HasElement(this IWebDriver driver, Func<IWebDriver, IWebElement> elementFinder)
        {
            try
            {
                elementFinder(driver);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }
}
