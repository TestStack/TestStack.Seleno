using System;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// The entry point for Seleno.
    /// </summary>
    public static class SelenoApplicationRunner
    {
        /// <summary>
        /// The currently running seleno application.
        /// </summary>
        public static ISelenoApplication Host { get; private set; }
       
        /// <summary>
        /// Begin a Seleno test for a Visual Studio web project.
        /// </summary>
        /// <param name="webProjectFolder">The name of the web project to run</param>
        /// <param name="portNumber">The port number to run the project under</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public static void Run(string webProjectFolder, 
                               int portNumber, 
                               Action<IAppConfigurator> configure = null)
        {
            var webApplication = new WebApplication(ProjectLocation.FromFolder(webProjectFolder), portNumber);
            Run(webApplication, configure);
        }

        /// <summary>
        /// Begin a Seleno test for a web application.
        /// </summary>
        /// <param name="app">The web application to test</param>
        /// <param name="configure">Any configuration changes you would like to make</param>
        public static void Run(WebApplication app, Action<IAppConfigurator> configure)
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
        public static void Run(Action<IAppConfigurator> configure)
        {
            Action<IAppConfigurator> action = x =>
            {
                if (configure != null)
                    configure(x);
            };

            Host = CreateApplication(action);
        }

        private static ISelenoApplication CreateApplication(Action<IAppConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = new AppConfigurator();
            configure(configurator);
            Host = configurator.CreateApplication();
            Host.Initialize();

            return Host;
        }
    }
}