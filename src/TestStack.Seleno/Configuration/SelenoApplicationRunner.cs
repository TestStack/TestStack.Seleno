using System;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration
{
    public static class SelenoApplicationRunner
    {
        static readonly ILog _log = LogManager.GetLogger("Seleno");
        public static ISelenoApplication Host { get; private set; }

        public static ISelenoApplication New(Action<IAppConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = new AppConfigurator();
            configure(configurator);
            Host = configurator.CreateApplication();

            return Host;
        }

        public static void Run(Action<IAppConfigurator> configure)
        {
            try
            {
                New(configure)
                    .Initialize();
            }
            catch (Exception ex)
            {
                _log.Error("The Seleno Application exited abnormally with an exception", ex);
            }
        }

    }
}