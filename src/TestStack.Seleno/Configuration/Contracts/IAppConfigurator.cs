using System;
using Castle.Core.Logging;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.Configuration.Contracts
{
    /// <summary>
    /// Configure a Seleno test.
    /// </summary>
    public interface IAppConfigurator
    {

        /// <summary>
        /// Specify the details of the project you are testing.
        /// </summary>
        /// <param name="webApplication">The project you are testing</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator ProjectToTest(WebApplication webApplication);

        /// <summary>
        /// Specify the web server you would like to use.
        /// By default the IISExpressWebServer is used.
        /// </summary>
        /// <param name="webServer">The webserver to use</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator WithWebServer(IWebServer webServer);

        /// <summary>
        /// Specify the web driver/browser you would like to use.
        /// By default Firefox is used.
        /// </summary>
        /// <param name="webDriver">The web driver</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator WithRemoteWebDriver(Func<RemoteWebDriver> webDriver);

        /// <summary>
        /// Specify the minimum amount of time in seconds to wait when trying to find elements on the page.
        /// By default the minimum wait is 10 seconds.
        /// </summary>
        /// <param name="minimumWaitInSeconds">The minimum number of seconds to wait to find an element on the page</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator WithMinimumWaitTimeoutInSecondsOf(int minimumWaitInSeconds);

        /// <summary>
        /// Specify the camera you would like to use.
        /// By default no camera is used.
        /// </summary>
        /// <param name="camera">The camera</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingCamera(ICamera camera);

        /// <summary>
        /// Specify the logger factory you would like to use.
        /// By default a null logger is used.
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory);
    }

    internal interface IInternalAppConfigurator : IAppConfigurator
    {
        ISelenoApplication CreateApplication();
    }
}