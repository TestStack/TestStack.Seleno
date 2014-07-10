using System;
using System.Web.Routing;
using Castle.Core.Logging;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.ControlIdGenerators;
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
        /// <param name="minimumWait">The minimum number of seconds to wait to find an element on the page</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator WithMinimumWaitTimeoutOf(TimeSpan minimumWait);

        /// <summary>
        /// Specify the camera you would like to use.
        /// By default no camera is used.
        /// </summary>
        /// <param name="camera">The camera</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingCamera(ICamera camera);

        /// <summary>
        /// Specify the directory path to store screenshots in.
        /// </summary>
        /// <param name="screenShotPath">The directory to store screenshots in</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingCamera(string screenShotPath);

        /// <summary>
        /// Specify the logger factory you would like to use.
        /// By default a null logger is used.
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory);

        /// <summary>
        /// Define the routes for the application.
        /// </summary>
        /// <param name="routeCollectionUpdater">A method that takes a route collection and populates it with routes</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator WithRouteConfig(Action<RouteCollection> routeCollectionUpdater);

        /// <summary>
        /// Specify a control id generator to use; default is <see cref="DefaultControlIdGenerator"/>, Seleno also has an
        ///   <see cref="MvcControlIdGenerator"/> and you can implement your own by implementing the <see cref="IControlIdGenerator"/> interface.
        /// </summary>
        /// <param name="controlIdGenerator">An instance of the <see cref="IControlIdGenerator"/> interface</param>
        /// <returns>The configurator to allow for method chaining</returns>
        IAppConfigurator UsingControlIdGenerator(IControlIdGenerator controlIdGenerator);

        /// <summary>
        /// Adds an environment variable to be injected into the web application process
        /// </summary>
        /// <param name="name">The name of the environment variable</param>
        /// <param name="value">The optional value of the environment variable</param>
        IAppConfigurator WithEnvironmentVariable(string name, string value = null);
    }

    internal interface IInternalAppConfigurator : IAppConfigurator
    {
        ISelenoApplication CreateApplication();
    }
}