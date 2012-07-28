using System;
using System.IO;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step2.Pages;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step2
{
    public class Application : Page
    {
        public Application(RemoteWebDriver browser)
        {
            Browser = browser;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Browser.Close();
        }

        public HomePage Start()
        {
            return Navigate().To<HomePage>(IISExpressRunner.HomePage);
        }

        public static HomePage HomePage
        {
            get
            {
                var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
                var solutionPath = dirInfo.Parent.Parent.Parent.FullName;
                var path = Path.Combine(solutionPath, "MvcMusicStore");
                IISExpressRunner.Start(path, 12345);
                var browser = new FirefoxDriver();

                browser.SetImplicitTimeout(10);
                var homePage = new Application(browser).Start();
                return homePage;
            }
        }
    }
}