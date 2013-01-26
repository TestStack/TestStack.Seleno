using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Navigator
{
    public class navigating_by_null_relative_url : SpecificationFor<PageNavigator>
    {
        private string _baseUrl = "some_url";

        public navigating_by_null_relative_url()
        {
            SelenoApplicationRunner.Host = Fake<ISelenoApplication>();
            Fake<IWebServer>().BaseUrl.Returns(_baseUrl);
        }

        public void when_navigating_by_relative_url_that_is_null()
        {
            SUT.To<TestPage>((string)null);
        }

        public void should_navigate_to_home_page()
        {
            Fake<IWebDriver>().Received().Navigate().GoToUrl(_baseUrl);
        }

        public override void InitialiseSystemUnderTest()
        {
            SUT = new PageNavigator(Fake<IWebDriver>(), Fake<IScriptExecutor>(), Fake<IWebServer>(), Fake<IComponentFactory>());
        }
    }

    public class navigating_by_relative_url : SpecificationFor<PageNavigator>
    {
        private string _baseUrl = "some_url";
        private string _relativeUrl = "something_random";

        public navigating_by_relative_url()
        {
            SelenoApplicationRunner.Host = Fake<ISelenoApplication>();
            Fake<IWebServer>().BaseUrl.Returns(_baseUrl);
        }

        public void when_navigating_by_relative_url()
        {
            SUT.To<TestPage>(_relativeUrl);
        }

        public void should_navigate_to_absolute_url()
        {
            Fake<IWebDriver>().Received().Navigate().GoToUrl(_baseUrl + _relativeUrl);
        }

        public override void InitialiseSystemUnderTest()
        {
            SUT = new PageNavigator(Fake<IWebDriver>(), Fake<IScriptExecutor>(), Fake<IWebServer>(), Fake<IComponentFactory>());
        }
    }
}
