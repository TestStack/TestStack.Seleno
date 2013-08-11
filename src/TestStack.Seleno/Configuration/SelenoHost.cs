using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Configuration
{
    /// <summary>Obsolete</summary>
    [Obsolete("Obsolete: Use SelenoHost instead. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
    public static class SelenoApplicationRunner
    {
        /// <summary>
        /// Obsolete
        /// </summary>
        public static void Run(string webProjectFolder, int portNumber, Action<IAppConfigurator> configure = null) {}
        /// <summary>
        /// Obsolete
        /// </summary>
        public static void Run(WebApplication app, Action<IAppConfigurator> configure) {}
        /// <summary>
        /// Obsolete
        /// </summary>
        public static void Run(Action<IAppConfigurator> configure) {}
    }

    // todo: Should there be a way to shutdown the current test programmatically to give more flexibility?
    /// <summary>
    /// The entry point for Seleno.
    /// </summary>
    public class SelenoHost : IDisposable
    {
        internal Func<IInternalAppConfigurator> AppConfiguratorFactory = () => new AppConfigurator();

        /// <summary>
        /// The currently running seleno application.
        /// </summary>
        // todo: should this be internal?
        public ISelenoApplication Application { get; set; }

        /// <summary>
        /// Begin a Seleno test for a Visual Studio web project.
        /// </summary>
        /// <param name="webProjectFolder">The name of the web project to run</param>
        /// <param name="portNumber">The port number to run the project under</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public void Run(string webProjectFolder, int portNumber, Action<IAppConfigurator> configure = null)
        {
            var webApplication = new WebApplication(ProjectLocation.FromFolder(webProjectFolder), portNumber);
            Run(webApplication, configure);
        }

        /// <summary>
        /// Begin a Seleno test for a web application.
        /// </summary>
        /// <param name="app">The web application to test</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public void Run(WebApplication app, Action<IAppConfigurator> configure)
        {
            Run(c =>
                {
                    c.ProjectToTest(app);
                    if (configure != null)
                        configure(c);
                }
            );
        }

        /// <summary>
        /// Begin a Seleno test.
        /// </summary>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public void Run(Action<IAppConfigurator> configure)
        {
            Action<IAppConfigurator> action = x =>
            {
                if (configure != null)
                    configure(x);
            };

            Application = CreateApplication(action);
            AppDomain.CurrentDomain.DomainUnload += CurrentDomainDomainUnload;
        }

        private void CurrentDomainDomainUnload(object sender, EventArgs e)
        {
            Application.Logger.Info("Starting domain unload");
            Application.Dispose();
            Application.Logger.Debug("Domain unloaded");
        }

        private ISelenoApplication CreateApplication(Action<IAppConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            if (Application != null)
                throw new SelenoException("You have already created a Seleno application; Seleno currently only supports one application at a time per app domain");

            var configurator = AppConfiguratorFactory();
            configure(configurator);
            Application = configurator.CreateApplication();
            Application.Initialize();

            return Application;
        }

        /// <summary>
        /// Navigate to the given controller action and return an initialised page object of the specified type.
        /// </summary>
        /// <typeparam name="TController">The controller to navigate to</typeparam>
        /// <typeparam name="TPage">The type of page object to initialise and return</typeparam>
        /// <param name="action">The controller action to navigate to</param>
        /// <returns>The initialised page object</returns>
        public TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            ThrowIfHostNotInitialised();
            return Application.NavigateToInitialPage<TController, TPage>(action);
        }

        /// <summary>
        /// Navigate to the given URL and return an initialised page object of the specified type.
        /// </summary>
        /// <typeparam name="TPage">The type of page object to initialise and return</typeparam>
        /// <param name="url">The URL to navigate to (either relative to the base URL of the site or an absolute URL)</param>
        /// <returns>The initialised page object</returns>
        public TPage NavigateToInitialPage<TPage>(string url = "") where TPage : UiComponent, new()
        {
            ThrowIfHostNotInitialised();
            return Application.NavigateToInitialPage<TPage>(url);
        }

        private void ThrowIfHostNotInitialised()
        {
            if (Application == null)
                throw new InvalidOperationException("You must call SelenoHost.Run(...) before using SelenoHost to navigate to a page.");
        }

        public void Dispose()
        {
            Application.Dispose();
        }
    }
}