using System;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4
{
    public abstract class ScenarioBase
    {
        static RemoteWebDriver _browser;
        protected RemoteWebDriver Browser
        {
            get
            {
                if (_browser == null)
                {
                    _browser = new FirefoxDriver();
                    _browser.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
                    PageFactory.Initialize(_browser);

                }
                return _browser;
            }
        }
    }
}