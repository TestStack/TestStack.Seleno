using Configurator = TestStack.BDDfy.Configuration.Configurator;
using TestStack.BDDfy.Processors.HtmlReporter;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

using NUnit.Framework;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests
{
    [SetUpFixture]
    public class AssemblySetupFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoApplicationRunner.Run("TestStack.Seleno.Samples.Movies", 19456);
            InitializeBDDfyReport();
        }

        private void InitializeBDDfyReport()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new FunctionalTestsHtmlReportConfig()));
        }
    }
}
