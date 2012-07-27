using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests
{
    [TestFixture]
    public class WebTestBase
    {
        protected RemoteWebDriver _driver;

        [SetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        
    }
}