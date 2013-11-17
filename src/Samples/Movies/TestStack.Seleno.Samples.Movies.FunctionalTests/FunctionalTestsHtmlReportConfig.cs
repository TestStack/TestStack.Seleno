using TestStack.BDDfy.Processors.Reporters.Html;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests
{
    public class FunctionalTestsHtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override string OutputFileName
        {
            get
            {
                return "MoviesFunctionalTests.html";
            }
        }

        public override string ReportHeader
        {
            get
            {
                return "Seleno Movie Sample";
            }
        }

        public override string ReportDescription
        {
            get
            {
                return "Functional Tests";
            }
        }
    }
}