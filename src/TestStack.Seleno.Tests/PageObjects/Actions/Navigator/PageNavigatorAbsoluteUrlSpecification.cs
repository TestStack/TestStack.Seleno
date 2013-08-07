using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Navigator
{
    abstract class PageNavigatorAbsoluteUrlSpecification : SpecificationFor<PageNavigator>
    {
        protected string _baseUrl = "http://localhost:12345/";
        protected string _relativeUrl = "something_random";

        public PageNavigatorAbsoluteUrlSpecification()
        {
            SubstituteFor<IWebServer>().BaseUrl.Returns(_baseUrl);
        }

        public void When_navigating_by_absolute_url()
        {
            SUT.To<TestPage>(_baseUrl + _relativeUrl);
        }
    }

    class navigating_by_absolute_url : PageNavigatorAbsoluteUrlSpecification
    {
        public void Then_should_navigate_to_absolute_url()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(_baseUrl + _relativeUrl);
        }
    }
}
