using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.TestObjects;
using FluentAssertions;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_navigating_to_url : SelenoApplicationSpecification
    {
        private const string _url = "/someurl";
        private TestPage _receivedPageObject;
        private readonly TestPage _expectedPageObject = new TestPage();

        public override void Setup()
        {
            SubstituteFor<IPageNavigator>().To<TestPage>(_url)
                .Returns(_expectedPageObject);
        }

        public void Given_initialised_application()
        {
            SUT.Initialize();
        }

        public void When_navigating_via_url()
        {
            _receivedPageObject = SUT.NavigateToInitialPage<TestPage>(_url);
        }

        public void Then_navigate_browser_to_site_home_and_return_page_object()
        {
            _receivedPageObject.Should().Be(_expectedPageObject);
        }
    }
}
