using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Seleno
{
    public static class WebDriverExtensions
    {
        public static RemoteWebDriver SetImplicitTimeout(this RemoteWebDriver driver, int timeoutInSeconds)
        {
            driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 0, timeoutInSeconds));
            return driver;
        }

        public static RemoteWebDriver SetWindowSize(this RemoteWebDriver driver, int width, int height)
        {
            driver.Manage().Window.Size = new Size(width, height);
            return driver;
        }

        public static RemoteWebDriver MaximizeWindow(this RemoteWebDriver driver)
        {
            driver.Manage().Window.Maximize();
            return driver;
        }

        public static RemoteWebDriver AddCookie(this RemoteWebDriver driver, Cookie cookieToAdd)
        {
            driver.Manage().Cookies.AddCookie(cookieToAdd);
            return driver;
        }

        public static RemoteWebDriver RemoveCookie(this RemoteWebDriver driver, Cookie cookieToRemove)
        {
            driver.Manage().Cookies.DeleteCookie(cookieToRemove);
            return driver;
        }
    }
}