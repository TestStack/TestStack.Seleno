//====================================================
//| Downloaded From                                  |
//| Visual C# Kicks - http://www.vcskicks.com/       |
//| License - http://www.vcskicks.com/license.php    |
//====================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleno.Locators
{
    public static class SeleniumExtensions
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
                      newScript.type = 'text/javascript';
                      newScript.src = '" + jQueryURL + @"';
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
        /// Overloads the FindElement function to include support for the jQuery selector class
        /// </summary>
        public static IWebElement FindElement(this RemoteWebDriver driver, By.jQueryBy by)
        {
            //First make sure we can use jQuery functions
            driver.LoadjQuery();

            //Execute the jQuery selector as a script
            IWebElement element = driver.ExecuteScript("return jQuery" + by.Selector + ".get(0)") as IWebElement;

            if (element != null)
                return element;
            else
                throw new NoSuchElementException("No element found with jQuery command: jQuery" + by.Selector);
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
