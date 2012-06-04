using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace MvcMusicStore.FunctionalTests
{
    class BrittleTests
    {
        private IWebDriver _driver;

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

        [Test]
        public void Can_buy_an_Album_when_registered()
        {
            _driver.Navigate().GoToUrl(SystemUnderTest.HomePageAddress);
            _driver.FindElement(By.LinkText("Admin")).Click();
            _driver.FindElement(By.LinkText("Register")).Click();
            _driver.FindElement(By.Id("UserName")).Clear();
            _driver.FindElement(By.Id("UserName")).SendKeys("HJSimpson");
            _driver.FindElement(By.Id("Email")).Clear();
            _driver.FindElement(By.Id("Email")).SendKeys("chunkylover53@aol.com");
            _driver.FindElement(By.Id("Password")).Clear();
            _driver.FindElement(By.Id("Password")).SendKeys("!2345Qwert");
            _driver.FindElement(By.Id("ConfirmPassword")).Clear();
            _driver.FindElement(By.Id("ConfirmPassword")).SendKeys("!2345Qwert");
            _driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
            _driver.FindElement(By.LinkText("Disco")).Click();
            _driver.FindElement(By.CssSelector("img[alt=\"Le Freak\"]")).Click();
            _driver.FindElement(By.LinkText("Add to cart")).Click();
            _driver.FindElement(By.LinkText("Checkout >>")).Click();
            _driver.FindElement(By.Id("FirstName")).Clear();
            _driver.FindElement(By.Id("FirstName")).SendKeys("Homer");
            _driver.FindElement(By.Id("LastName")).Clear();
            _driver.FindElement(By.Id("LastName")).SendKeys("Simpson");
            _driver.FindElement(By.Id("Address")).Clear();
            _driver.FindElement(By.Id("Address")).SendKeys("742 Evergreen Terrace");
            _driver.FindElement(By.Id("City")).Clear();
            _driver.FindElement(By.Id("City")).SendKeys("Springfield");
            _driver.FindElement(By.Id("State")).Clear();
            _driver.FindElement(By.Id("State")).SendKeys("Kentucky");
            _driver.FindElement(By.Id("PostalCode")).Clear();
            _driver.FindElement(By.Id("PostalCode")).SendKeys("123456");
            _driver.FindElement(By.Id("Country")).Clear();
            _driver.FindElement(By.Id("Country")).SendKeys("United States");
            _driver.FindElement(By.Id("Phone")).Clear();
            _driver.FindElement(By.Id("Phone")).SendKeys("2341231241");
            _driver.FindElement(By.Id("Email")).Clear();
            _driver.FindElement(By.Id("Email")).SendKeys("chunkylover53@aol.com");
            _driver.FindElement(By.Id("PromoCode")).Clear();
            _driver.FindElement(By.Id("PromoCode")).SendKeys("FREE");
            _driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();

            Assert.IsTrue(_driver.PageSource.Contains("Checkout Complete"));
        }
    }
}