using NUnit.Framework;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Browsers
{
    abstract class BrowserTest
    {
        protected abstract RemoteWebDriver WebDriver { get; }

        [Explicit]
        [Test]
        public void RunTest()
        {
            using (var host = new SelenoHost())
            {
                host.Run(
                    x =>
                    x.WithRemoteWebDriver(() => WebDriver)
                     .WithWebServer(new InternetWebServer("https://www.google.com/")));
                var title = host.NavigateToInitialPage<Page>().Title;

                Assert.That(title, Is.EqualTo("Google"));
            }
        }
    }

    class FirefoxTest : BrowserTest
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.FireFox(); }
        }
    }

    class ChromeTest : BrowserTest
    {
        protected override RemoteWebDriver WebDriver
        {
            get { return BrowserFactory.Chrome(); }
        }
    }

	class IETest : BrowserTest
	{
		protected override RemoteWebDriver WebDriver
		{
			get { return BrowserFactory.InternetExplorer(); }
		}
	}

	class IETestWithOptions : BrowserTest
	{
		protected override RemoteWebDriver WebDriver
		{
			get
			{
				var options = new InternetExplorerOptions { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
				return BrowserFactory.InternetExplorer(options);
			}
		}
	}
}
