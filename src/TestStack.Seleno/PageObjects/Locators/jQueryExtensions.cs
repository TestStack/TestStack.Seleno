using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.PageObjects.Locators
{
    public static class jQueryExtensions
    {
        /// <summary>
        /// Return whether jQuery is loaded in the current page
        /// </summary>
        public static bool jQueryLoaded(this RemoteWebDriver driver)
        {
            bool result = false;
            try
            {
                result = (bool)driver.ExecuteScript("return typeof jQuery == 'function'");
            }
            catch (WebDriverException)
            {
            }

            return result;
        }

        public static void LoadjQuery(this IWebDriver driver, string version = "latest", TimeSpan? timeout = null)
        {
            var remoteWebDriver = driver as RemoteWebDriver;
            if (remoteWebDriver != null)
            {
                remoteWebDriver.LoadjQuery(version, timeout);
            }
        }

        /// <summary>
        /// Load jQuery from an external URL to the current page
        /// </summary>
        public static void LoadjQuery(this RemoteWebDriver driver, string version = "any", TimeSpan? timeout = null)
        {
            //Get the url to load jQuery from
            string jQueryURL = "";
            if (version == "" || version.ToLower() == "latest")
                jQueryURL = "http://code.jquery.com/jquery-latest.min.js";
            else
                jQueryURL = "https://ajax.googleapis.com/ajax/libs/jquery/" + version + "/jquery.min.js";

            //Script to load jQuery from external site
            string versionEnforceScript = version.ToLower() != "any" ? string.Format("if (typeof jQuery == 'function' && jQuery.fn.jquery != '{0}') jQuery.noConflict(true);", version)
                                              : string.Empty;
            string loadingScript =
                @"if (typeof jQuery != 'function')
                  {
                      var headID = document.getElementsByTagName('head')[0];
                      var newScript = document.createElement('script');
                      newScript.setAttribute('type', 'text/javascript');
                      newScript.setAttribute('src' , '" + jQueryURL + @"');
                      headID.appendChild(newScript);
                  }
                  return (typeof jQuery == 'function');";

            bool loaded = (bool)driver.ExecuteScript(versionEnforceScript + loadingScript);

            if (!loaded)
            {
                //Wait for the script to load
                //Verify library loaded
                if (!timeout.HasValue)
                    timeout = new TimeSpan(0, 0, 30);

                int timePassed = 0;
                while (!driver.jQueryLoaded())
                {
                    Thread.Sleep(500);
                    timePassed += 500;

                    if (timePassed > timeout.Value.TotalMilliseconds)
                        throw new Exception("Could not load jQuery");
                }
            }

            string v = driver.ExecuteScript("return jQuery.fn.jquery").ToString();
        }

        /// <summary>
        /// Gets the HTML using jQuery selector class
        /// </summary>
        public static string GetHtml(this IWebDriver driver, By.jQueryBy by)
        {
            return (driver as RemoteWebDriver).GetHtml(by);
        }

        /// <summary>
        /// Overloads the FindElement function to include support for the jQuery selector class
        /// </summary>
        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By.jQueryBy by)
        {
            return (driver as RemoteWebDriver).FindElements(by);
        }

        public static int CountNumberOfElements(this IWebDriver browser, By.jQueryBy by, Func<IWebElement, Boolean> predicate = null)
        {
            var elements = browser.FindElements(by);
            if (predicate == null)
            {
                return elements.Count;
            }

            return elements.Where(predicate).Count();
        }


        /// <summary>
        /// Overloads the FindElement function to include support for the jQuery selector class
        /// </summary>
        public static IWebElement FindElementByjQuery(this IWebDriver driver, By.jQueryBy by)
        {
            var browser = (RemoteWebDriver) driver;
            //First make sure we can use jQuery functions
            browser.LoadjQuery();

            //Execute the jQuery selector as a script
            IWebElement element = browser.ExecuteScript("return jQuery" + by.Selector + ".get(0)") as IWebElement;

            if (element != null)
                return element;
            else
                throw new NoSuchElementException("No element found with jQuery command: jQuery" + by.Selector);
        }

        /// <summary>
        /// Overloads the FindElement function to include support for the jQuery selector class
        /// </summary>
        public static string GetHtml(this RemoteWebDriver driver, By.jQueryBy by)
        {
            //First make sure we can use jQuery functions
            driver.LoadjQuery();

            //Execute the jQuery selector as a script
            var html = driver.ExecuteScript("return jQuery" + by.Selector + ".html()").ToString();
            return html;
        }

        /// <summary>
        /// Overloads the FindElements function to include support for the jQuery selector class
        /// </summary>
        public static ReadOnlyCollection<IWebElement> FindElements(this RemoteWebDriver driver, By.jQueryBy by)
        {
            //First make sure we can use jQuery functions
            driver.LoadjQuery();

            //Execute the jQuery selector as a script
            ReadOnlyCollection<IWebElement> collection = driver.ExecuteScript("return jQuery" + by.Selector + ".get()") as ReadOnlyCollection<IWebElement>;

            //Unlike FindElement, FindElements does not throw an exception if no elements are found
            //and instead returns an empty list
            if (collection == null)
                collection = new ReadOnlyCollection<IWebElement>(new List<IWebElement>()); //empty list

            return collection;
        }
    }
}
