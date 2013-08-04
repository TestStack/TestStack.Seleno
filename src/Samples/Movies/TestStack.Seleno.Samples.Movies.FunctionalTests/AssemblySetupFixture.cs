using Configurator = TestStack.BDDfy.Configuration.Configurator;
using TestStack.BDDfy.Processors.HtmlReporter;
using TestStack.Seleno.Configuration;
using NUnit.Framework;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests
{
    public class Host
    {
        public static readonly SelenoHost Instance = new SelenoHost();
    }


    [SetUpFixture]
    public class AssemblySetupFixture
    {

        [SetUp]
        public void SetUp()
        {
            Host.Instance.Run("TestStack.Seleno.Samples.Movies", 19456);
            InitializeBDDfyReport();
        }

        private void InitializeBDDfyReport()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new FunctionalTestsHtmlReportConfig()));
        }
    }
}
