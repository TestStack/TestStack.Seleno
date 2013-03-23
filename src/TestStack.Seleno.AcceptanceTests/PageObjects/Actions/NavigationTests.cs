using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.Configuration;
using FluentAssertions;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Actions
{
    abstract class NavigationTests
    {
        public class Navigating_via_url : NavigationTests
        {
            public void When_navigating_by_url()
            {
                ResultPage = Page.GoToReadModelPageByUrl();
            }
        }

        public class Navigating_via_mvc : NavigationTests
        {
            public void When_navigating_by_mvc_action()
            {
                ResultPage = Page.GoToReadModelPageByMvcAction();
            }
        }

        public class Navigating_via_link : NavigationTests
        {
            public void When_navigating_by_clicking_a_link()
            {
                ResultPage = Page.GoToReadModelPageByLink();
            }
        }

        public class Navigating_via_button : NavigationTests
        {
            public void When_navigating_by_clicking_a_button()
            {
                ResultPage = Page.GoToReadModelPageByButton();
            }
        }

        protected HomePage Page;
        protected Form1Page ResultPage;

        public void Given_browser_is_on_a_page()
        {
            Page = SelenoHost.NavigateToInitialPage<HomePage>();
        }

        public void Then_the_page_was_navigated_to()
        {
            ResultPage.Title.Should().Be("FixtureA");
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}
