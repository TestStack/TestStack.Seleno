using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    class When_navigating_to_initial_page_from_controller_action : SelenoHostSpecification
    {
        private readonly Expression<Action<TestController>> _controllerAction = x => x.Index();

        public void Given_the_Seleno_Application_is_configured()
        {
            SUT.Run(c => {});
        }
        
        public void When_navigating_to_initial_page()
        {
            SUT.NavigateToInitialPage<TestController, TestPage>(_controllerAction);
        }
        
        public void Then_it_should_navigate_using_the_seleno_application()
        {
            SelenoApplication
                .Received()
                .NavigateToInitialPage<TestController, TestPage>(_controllerAction);
        }
    }

    class When_navigating_to_initial_page_from_controller_action_with_dictionary : SelenoHostSpecification
    {
        private readonly Expression<Action<TestController>> _controllerAction = x => x.Index();
        private readonly Dictionary<string, object> _routeValues = new Dictionary<string, object>();

        public void Given_the_Seleno_Application_is_configured()
        {
            SUT.Run(c => { });
        }

        public void When_navigating_to_initial_page()
        {
            SUT.NavigateToInitialPage<TestController, TestPage>(_controllerAction, _routeValues);
        }

        public void Then_it_should_navigate_using_the_seleno_application()
        {
            SelenoApplication
                .Received()
                .NavigateToInitialPage<TestController, TestPage>(_controllerAction, _routeValues);
        }
    }

    class When_navigating_to_initial_page_from_controller_action_with_object : SelenoHostSpecification
    {
        private readonly Expression<Action<TestController>> _controllerAction = x => x.Index();
        private readonly object _routeValues = new {a = "b"};

        public void Given_the_Seleno_Application_is_configured()
        {
            SUT.Run(c => { });
        }

        public void When_navigating_to_initial_page()
        {
            SUT.NavigateToInitialPage<TestController, TestPage>(_controllerAction, _routeValues);
        }

        public void Then_it_should_navigate_using_the_seleno_application()
        {
            SelenoApplication
                .Received()
                .NavigateToInitialPage<TestController, TestPage>(_controllerAction, _routeValues);
        }
    }
}
