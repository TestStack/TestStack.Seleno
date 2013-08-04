using System;
using Castle.Core.Logging;
using NUnit.Framework;
using TestStack.Seleno.AcceptanceTests.Web.App_Start;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests
{
    public class Host
    {
        public static readonly SelenoHost Instance = new SelenoHost();
    }

    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            Host.Instance.Run("TestStack.Seleno.AcceptanceTests.Web", 12346, c => c
                .UsingLoggerFactory(new ConsoleFactory())
                .WithMinimumWaitTimeoutOf(TimeSpan.FromSeconds(1))
                .WithRouteConfig(RouteConfig.RegisterRoutes)
            );
        }
    }
}
