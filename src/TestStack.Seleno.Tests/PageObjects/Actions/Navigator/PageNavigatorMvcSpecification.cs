using System.Collections.Generic;
using System.Web.Routing;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;
using NSubstitute;
using System.Web.Mvc;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Navigator
{
    abstract class PageNavigatorMvcSpecification : SpecificationFor<PageNavigator>
    {
        protected const string BaseUrl = "http://localhost/";

        public override void InitialiseSystemUnderTest()
        {
            DefineRoutes(SubstituteFor<RouteCollection>());
            SubstituteFor<IWebServer>().BaseUrl.Returns(BaseUrl);

            base.InitialiseSystemUnderTest();
        }

        protected abstract void DefineRoutes(RouteCollection routes);
    }

    class When_navigating_to_parameterless_action : PageNavigatorMvcSpecification
    {
        public void When_navigating_by_mvc_action()
        {
            SUT.To<TestController, TestPage>(c => c.Index());
        }

        public void Then_go_to_expected_url_route()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(string.Format("{0}testindex", BaseUrl));
        }

        protected override void DefineRoutes(RouteCollection routes)
        {
            routes.MapRoute("Index", "testindex", new { controller = "Test", action = "Index" });
        }
    }

    class When_navigating_to_action_with_in_url_route_parameters : PageNavigatorMvcSpecification
    {
        public void When_navigating_by_mvc_action()
        {
            SUT.To<TestController, TestPage>(c => c.ActionWithParameters("hi"));
        }

        public void Then_go_to_expected_url_route()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(string.Format("{0}action/hi", BaseUrl));
        }

        protected override void DefineRoutes(RouteCollection routes)
        {
            routes.MapRoute("ActionWithParameters", "action/{parameter}", new { controller = "Test", action = "ActionWithParameters" });
        }
    }

    class When_navigating_to_action_with_query_string_route_parameters : PageNavigatorMvcSpecification
    {
        public void When_navigating_by_mvc_action()
        {
            SUT.To<TestController, TestPage>(c => c.ActionWithParameters("hi"));
        }

        public void Then_go_to_expected_url_route()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(string.Format("{0}action/url?parameter=hi", BaseUrl));
        }

        protected override void DefineRoutes(RouteCollection routes)
        {
            routes.MapRoute("ActionWithParameters", "action/url", new { controller = "Test", action = "ActionWithParameters" });
        }
    }

    class When_navigating_to_action_with_extra_route_parameter : PageNavigatorMvcSpecification
    {
        public void When_navigating_by_mvc_action()
        {
            SUT.To<TestController, TestPage>(c => c.ActionWithParameters("hi"), new Dictionary<string, object>{{"other", "value"}});
        }

        public void Then_go_to_expected_url_route()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(string.Format("{0}action/hi?other=value", BaseUrl));
        }

        protected override void DefineRoutes(RouteCollection routes)
        {
            routes.MapRoute("ActionWithParameters", "action/{parameter}", new { controller = "Test", action = "ActionWithParameters" });
        }
    }

    class When_navigating_to_action_with_expected_route_parameter : PageNavigatorMvcSpecification
    {
        public void When_navigating_by_mvc_action()
        {
            SUT.To<TestController, TestPage>(c => c.ActionWithParameters("hi"), new { other = "value", id = 123 });
        }

        public void Then_go_to_expected_url_route()
        {
            SubstituteFor<IWebDriver>().Navigate().Received().GoToUrl(string.Format("{0}action/hi/123?other=value", BaseUrl));
        }

        protected override void DefineRoutes(RouteCollection routes)
        {
            routes.MapRoute("ActionWithParameters", "action/{parameter}/{id}", new { controller = "Test", action = "ActionWithParameters" });
        }
    }
}
