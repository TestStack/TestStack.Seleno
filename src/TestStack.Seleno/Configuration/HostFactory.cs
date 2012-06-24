using System;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Infrastructure.Logging;

namespace TestStack.Seleno.Configuration
{
    public static class HostFactory
    {
        static readonly ILog _log = LogManager.GetLogger("Topshelf");
        public static IHost SelenoHost { get; private set; }

        public static IHost New(Action<IHostConfigurator> configure)
        {
            if (configure == null)
                throw new ArgumentNullException("configure");

            var configurator = new HostConfigurator();

            configure(configurator);

            //configurator.ApplyCommandLine();

            configurator.Validate();

            SelenoHost = configurator.CreateHost();
            return SelenoHost;
        }

        public static void Run(Action<IHostConfigurator> configure)
        {
            try
            {
                New(configure)
                    .Initialize();
            }
            catch (Exception ex)
            {
                _log.Error("The service exited abnormally with an exception", ex);
            }
        }

    }
}