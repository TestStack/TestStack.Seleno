using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.TestObjects;
using FluentAssertions;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_navigating_to_mvc_controller : SelenoApplicationSpecification
    {
        private Expression<Action<TestController>> _action;
        private TestPage _receivedPageObject;
        private readonly TestPage _expectedPageObject = new TestPage();

        public override void Setup()
        {
            _action = c => c.Index();
            SubstituteFor<IPageNavigator>().To<TestController, TestPage>(_action)
                .Returns(_expectedPageObject);
        }

        public void Given_initialised_application()
        {
            SUT.Initialize();
        }

        public void When_navigating_via_mvc_controller()
        {
            _receivedPageObject = SUT.NavigateToInitialPage<TestController, TestPage>(_action);
        }

        public void Then_navigate_browser_to_site_home_and_return_page_object()
        {
            _receivedPageObject.Should().Be(_expectedPageObject);
        }
    }

    class When_navigating_to_mvc_controller_with_dictionary : SelenoApplicationSpecification
    {
        private Expression<Action<TestController>> _action;
        private TestPage _receivedPageObject;
        private readonly TestPage _expectedPageObject = new TestPage();
        private readonly Dictionary<string, object> _routeValues = new Dictionary<string, object>();

        public override void Setup()
        {
            _action = c => c.Index();
            SubstituteFor<IPageNavigator>().To<TestController, TestPage>(_action, _routeValues)
                .Returns(_expectedPageObject);
        }

        public void Given_initialised_application()
        {
            SUT.Initialize();
        }

        public void When_navigating_via_mvc_controller()
        {
            _receivedPageObject = SUT.NavigateToInitialPage<TestController, TestPage>(_action, _routeValues);
        }

        public void Then_navigate_browser_to_site_home_and_return_page_object()
        {
            _receivedPageObject.Should().Be(_expectedPageObject);
        }
    }


    class When_navigating_to_mvc_controller_with_object : SelenoApplicationSpecification
    {
        private Expression<Action<TestController>> _action;
        private TestPage _receivedPageObject;
        private readonly TestPage _expectedPageObject = new TestPage();
        private readonly object _routeValues = new {a = "b"};

        public override void Setup()
        {
            _action = c => c.Index();
            SubstituteFor<IPageNavigator>().To<TestController, TestPage>(_action, _routeValues)
                .Returns(_expectedPageObject);
        }

        public void Given_initialised_application()
        {
            SUT.Initialize();
        }

        public void When_navigating_via_mvc_controller()
        {
            _receivedPageObject = SUT.NavigateToInitialPage<TestController, TestPage>(_action, _routeValues);
        }

        public void Then_navigate_browser_to_site_home_and_return_page_object()
        {
            _receivedPageObject.Should().Be(_expectedPageObject);
        }
    }
}
