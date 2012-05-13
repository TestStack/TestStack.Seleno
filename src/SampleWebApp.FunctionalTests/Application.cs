using System;
using System.IO;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using SeleniumExtensions;

namespace SampleWebApp.FunctionalTests
{
    public class Application : UiComponent
    {
        public Application(RemoteWebDriver browser)
        {
            Browser = browser;
            MvcApplication.RegisterRoutes();

            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Browser.Close();
        }

        public HomePage Start()
        {
            return NavigateTo<HomePage>(IISExpressRunner.HomePage);
        }

        public static HomePage HomePage
        {
            get
            {
                var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
                var solutionPath = dirInfo.Parent.Parent.Parent.FullName;
                var path = Path.Combine(solutionPath, "SampleWebApp");
                IISExpressRunner.Start(path, 12345);
                var options = new InternetExplorerOptions();
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                var browser = new InternetExplorerDriver(options);

                browser.SetImplicitTimeout(10);
                var homePage = new Application(browser).Start();
                return homePage;
            }
        }
    }
}