using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    [Obsolete("Please use SelenoHost instead of SelenoApplicationRunner", true)]
    public static class SelenoApplicationRunner
    {
        public static void Run(string webProjectFolder, int portNumber, Action<IAppConfigurator> configure = null) {}
        public static void Run(WebApplication app, Action<IAppConfigurator> configure) {}
        public static void Run(Action<IAppConfigurator> configure) {}
    }

    /// <summary>
    /// The entry point for Seleno.
    /// </summary>
    public static class SelenoHost
    {
        /// <summary>
        /// The currently running seleno application.
        /// </summary>
        public static ISelenoApplication Host { get; internal set; }
       
        /// <summary>
        /// Begin a Seleno test for a Visual Studio web project.
        /// </summary>
        /// <param name="webProjectFolder">The name of the web project to run</param>
        /// <param name="portNumber">The port number to run the project under</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public static void Run(string webProjectFolder, 
                               int portNumber, 
                               Action<IAppConfigurator> configure = null,
                               IAppConfigurator appConfigurator = null)
        {
            var webApplication = new WebApplication(ProjectLocation.FromFolder(webProjectFolder), portNumber);
            Run(webApplication, configure, appConfigurator);
        }

        /// <summary>
        /// Begin a Seleno test for a web application.
        /// </summary>
        /// <param name="app">The web application to test</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public static void Run(WebApplication app, Action<IAppConfigurator> configure, IAppConfigurator appConfigurator = null)
        {
            Run(c =>
                {
                    c.ProjectToTest(app);
                    if (configure != null)
                        configure(c);
                }, appConfigurator
            );
        }

        /// <summary>
        /// Begin a Seleno test.
        /// </summary>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public static void Run(Action<IAppConfigurator> configure = null, IAppConfigurator appConfigurator = null)
        {
            Action<IAppConfigurator> action = x =>
            {
                if (configure != null)
                    configure(x);
            };

            Host = CreateApplication(action, appConfigurator);
        }

        private static ISelenoApplication CreateApplication(Action<IAppConfigurator> configure, IAppConfigurator appConfigurator  = null)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = appConfigurator ?? new AppConfigurator();
            configure(configurator);
            Host = configurator.CreateApplication();
            Host.Initialize();

            return Host;
        }

        /// <summary>
        /// Navigate to the given controller action and return an initialised page object of the specified type.
        /// </summary>
        /// <typeparam name="TController">The controller to navigate to</typeparam>
        /// <typeparam name="TPage">The type of page object to initialise and return</typeparam>
        /// <param name="action">The controller action to navigate to</param>
        /// <returns>The initialised page object</returns>
        public static TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            ThrowIfHostNotInitialised();
            return Host.Container.Resolve<IPageNavigator>().To<TController, TPage>(action);
        }

        /// <summary>
        /// Navigate to the given URL and return an initialised page object of the specified type.
        /// </summary>
        /// <typeparam name="TPage">The type of page object to initialise and return</typeparam>
        /// <param name="relativeUrl">The URL to navigate to (relative to the base URL of the site)</param>
        /// <returns>The initialised page object</returns>
        public static TPage NavigateToInitialPage<TPage>(string relativeUrl = "") where TPage : UiComponent, new()
        {
            ThrowIfHostNotInitialised();
            return Host.Container.Resolve<IPageNavigator>().To<TPage>(relativeUrl);
        }

        private static void ThrowIfHostNotInitialised()
        {
            if (Host == null)
                throw new InvalidOperationException("You must call SelenoHost.Run(...) before using SelenoHost to navigate to a page.");
        }
    }
}