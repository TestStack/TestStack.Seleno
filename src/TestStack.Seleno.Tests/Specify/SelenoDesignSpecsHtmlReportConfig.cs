using TestStack.BDDfy.Processors.HtmlReporter;

namespace TestStack.Seleno.Tests.Specify
{
    public class SelenoDesignSpecsHtmlReportConfig : DefaultHtmlReportConfiguration
    {
        public override string OutputFileName
        {
            get
            {
                return "SelenoSpecifications.html";
            }
        }

        public override string ReportHeader
        {
            get
            {
                return "Seleno ~ the simplest web UI automation framework EVER!";
            }
        }

        public override string ReportDescription
        {
            get
            {
                return "Design Specifications";
            }
        }
    }
}