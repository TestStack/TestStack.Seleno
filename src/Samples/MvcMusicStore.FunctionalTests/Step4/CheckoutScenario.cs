using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.BDDfy.Core;

namespace MvcMusicStore.FunctionalTests.Step4
{
    [Story(
        AsA="As a Customer",
        IWant="I want to purchase an album",
        SoThat="So that I can enjoy my music")]
    abstract class CheckoutScenario : WebTestBase
    {
        protected abstract string ScenarioTitle { get; }

        [Test]
        public void BddifyMe()
        {
            this.BDDfy(ScenarioTitle);
        }

        public void Given_that_I_am_a_logged_in_user()
        {
            _driver.Navigate().GoToUrl(SystemUnderTest.HomePageAddress);
        }
    }
}
