using Castle.Core.Logging;
using NUnit.Framework;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoHost.Run("TestStack.Seleno.AcceptanceTests.Web", 12346, c => c
                .UsingLoggerFactory(new ConsoleFactory())
                .WithPageObjectsFrom(typeof(HomePage).Assembly)
            );
        }
    }
}
