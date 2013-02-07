using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Navigator
{
    public abstract class PageNavigatorRelativeUrlSpecification : SpecificationFor<PageNavigator>
    {
        protected string _baseUrl = "some_url";
        protected string _relativeUrl = "something_random";

        public PageNavigatorRelativeUrlSpecification()
        {
            SubstituteFor<IWebServer>().BaseUrl.Returns(_baseUrl);
        }

        public void When_navigating_by_relative_url()
        {
            SUT.To<TestPage>(_relativeUrl);
        }
    }

    public class navigating_by_null_relative_url : PageNavigatorRelativeUrlSpecification
    {
        public void Given_the_relative_url_is_null()
        {
            _relativeUrl = (string) null;
        }

        public void Then_should_navigate_to_home_page()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(_baseUrl);
        }
    }

    public class navigating_by_relative_url : PageNavigatorRelativeUrlSpecification
    {
        public void Then_should_navigate_to_absolute_url()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(_baseUrl + _relativeUrl);
        }
    }
}
