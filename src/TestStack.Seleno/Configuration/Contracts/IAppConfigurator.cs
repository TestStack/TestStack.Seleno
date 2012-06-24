using System;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface IAppConfigurator
    {
        void ProjectToTest(WebApplication webApplication);
        void WithWebServer(IWebServer webServer);
        void WithWebDriver(Func<IWebDriver> webDriver);
        void UsingCamera(ICamera camera);
        void UsingLogger(ILogFactory logFactory);
    }
}