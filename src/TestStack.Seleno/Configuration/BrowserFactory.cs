using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration
{
    public static class BrowserFactory
    {
        public static IWebDriver FireFox()
        {
            var browser = new FirefoxDriver();
            browser.SetImplicitTimeout(10);
            return browser;
        }

        public static IWebDriver InternetExplorer()
        {
            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            return new InternetExplorerDriver(options);
        }
    }
}