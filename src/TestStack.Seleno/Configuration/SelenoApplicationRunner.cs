using System;

using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration
{
    public static class SelenoApplicationRunner
    {
        static readonly ILog _log = LogManager.GetLogger("Seleno");
        public static ISelenoApplication Host { get; private set; }

        private static ISelenoApplication New(Action<IAppConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = new AppConfigurator();
            configure(configurator);
            Host = configurator.CreateApplication();
            Host.Initialize();

            return Host;
        }

        public static void Run(string webProjectFolder, int portNumber)
        {
            var webApplication = new WebApplication(ProjectLocation.FromFolder(webProjectFolder), portNumber);
            Host = New(x => x.ProjectToTest(webApplication));
        }

        public static void Run(WebApplication app, Action<IAppConfigurator> configure)
        {
            try
            {
                Action<IAppConfigurator> action = x =>
                {
                    x.ProjectToTest(app);
                    configure(x);
                };
                
               Host = New(action);
            }
            catch (Exception ex)
            {
                _log.Error("The Seleno Application exited abnormally with an exception", ex);
            }
        }

    }
}