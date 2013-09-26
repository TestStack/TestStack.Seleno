using NUnit.Framework;
using TestStack.BDDfy;
using FluentAssertions;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.ActionsTests
{
    abstract class NavigationTests
    {
        public class Navigating_via_relative_url : NavigationTests
        {
            public void When_navigating_by_url()
            {
                ResultPage = Page.GoToReadModelPageByRelativeUrl();
            }
        }

        public class Navigating_via_absolute_url : NavigationTests
        {
            public void When_navigating_by_url()
            {
                ResultPage = Page.GoToReadModelPageByAbsoluteUrl();
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
            Page = Host.Instance.NavigateToInitialPage<HomePage>();
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
