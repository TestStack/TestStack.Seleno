using NUnit.Framework;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests
{
    [SetUpFixture]
    public class AssemblySetupFixtue
    {
        [SetUp]
        public void InitialiseAppDomain()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            Configurator.BatchProcessors.Add(new HtmlReporter(new SelenoDesignSpecsHtmlReportConfig()));
            Configurator.Scanners.StoryMetadataScanner = () => new SpecStoryMetaDataScanner();
        }
    }
}
