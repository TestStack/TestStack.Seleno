using System;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;
using NSubstitute;
using FluentAssertions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Navigator
{
    class PageNavigatorJQueryBySpecs : SpecificationFor<PageNavigator>
    {
        private TestPage _page;
        private readonly TestPage _expectedPage = new TestPage();
        private readonly By.jQueryBy _jquerySelector = By.jQuery("selector");
        private Action<IWebElement> _action;

        public override void EstablishContext()
        {
            SubstituteFor<IComponentFactory>().CreatePage<TestPage>().Returns(_expectedPage);
            SubstituteFor<IExecutor>().ActionOnLocator(Arg.Any<By.jQueryBy>(), Arg.Do<Action<IWebElement>>(a => _action = a));
        }

        public void When_navigating_by_jquery()
        {
            _page = SUT.To<TestPage>(_jquerySelector);
        }

        public void Then_navigate_using_jquery_selector()
        {
            SubstituteFor<IExecutor>().Received().ActionOnLocator(_jquerySelector, Arg.Any<Action<IWebElement>>());
            var webElement = Substitute.For<IWebElement>();
            _action(webElement);
            webElement.Received().Click();
        }

        public void And_return_initialised_page()
        {
            _page.Should().Be(_expectedPage);
        }
    }
}
