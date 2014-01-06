using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Fakes;
using Microsoft.Web.Mvc;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageNavigator : IPageNavigator
    {
        protected IWebDriver Browser;
        readonly IExecutor _executor;
        private readonly IWebServer _webServer;
        readonly IComponentFactory _componentFactory;
        private readonly RouteCollection _routeCollection;

        public PageNavigator(IWebDriver browser, IExecutor executor, IWebServer webServer, IComponentFactory componentFactory, RouteCollection routeCollection)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (executor == null) throw new ArgumentNullException("executor");
            Browser = browser;
            _executor = executor;
            _webServer = webServer;
            _componentFactory = componentFactory;
            _routeCollection = routeCollection;
        }

        public TPage To<TPage>(By clickDestination, TimeSpan maxWait = default(TimeSpan)) where TPage : UiComponent, new()
        {
            _executor.ActionOnLocator(clickDestination, e => e.Click(), maxWait);
            return _componentFactory.CreatePage<TPage>();
        }

        public TPage To<TPage>(Locators.By.jQueryBy jQueryElementToClick, TimeSpan maxWait = default(TimeSpan)) where TPage : UiComponent, new()
        {
            _executor.ActionOnLocator(jQueryElementToClick, e => e.Click(), maxWait);
            return _componentFactory.CreatePage<TPage>();
        }

        public TPage To<TPage>(string url) where TPage : UiComponent, new()
        {
            string targetPageUrl = new Uri(new Uri(_webServer.BaseUrl), url).ToString();
            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                targetPageUrl = uri.AbsoluteUri;

            Browser.Navigate().GoToUrl(targetPageUrl);
            return _componentFactory.CreatePage<TPage>();
        }

        // todo: Move to a Seleno.Mvc project if established
        public TPage To<TController, TPage>(Expression<Action<TController>> action, IDictionary<string, object> routeValues = null)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            var requestContext = new RequestContext(FakeHttpContext.Root(), new RouteData());

            var actionRouteValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            if (routeValues != null)
                foreach (var v in routeValues)
                    actionRouteValues[v.Key] = v.Value;

            var urlHelper = new UrlHelper(requestContext, _routeCollection);
            var relativeUrl = urlHelper.RouteUrl(new RouteValueDictionary(actionRouteValues));

            return To<TPage>(relativeUrl);
        }

        public TPage To<TController, TPage>(Expression<Action<TController>> action, object routeValues)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            return To<TController, TPage>(action, new RouteValueDictionary(routeValues));
        }

        public void To(By clickDestination)
        {
            _executor.ActionOnLocator(clickDestination, e => e.Click());
        }
    }
}
