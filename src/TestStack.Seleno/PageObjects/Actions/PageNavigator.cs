using System;
using System.Linq.Expressions;
using System.Web.Mvc;
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

        public PageNavigator(IWebDriver browser, IExecutor executor, IWebServer webServer, IComponentFactory componentFactory)
        {
            if (browser == null) throw new ArgumentNullException("browser");
            if (executor == null) throw new ArgumentNullException("executor");
            Browser = browser;
            _executor = executor;
            _webServer = webServer;
            _componentFactory = componentFactory;
        }

        public TPage To<TPage>(By clickDestination) where TPage : UiComponent, new()
        {
            To(clickDestination);
            return _componentFactory.CreatePage<TPage>();
        }

        public TPage To<TPage>(string relativeUrl) where TPage : UiComponent, new()
        {
            Browser.Navigate().GoToUrl(_webServer.BaseUrl + relativeUrl);
            return _componentFactory.CreatePage<TPage>();
        }

        // This will move to MVC project once that is established
        public TPage To<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            var helper = new HtmlHelper(new ViewContext { HttpContext = FakeHttpContext.Root() }, new FakeViewDataContainer());
            var relativeUrl = helper.BuildUrlFromExpression(action);

            return To<TPage>(_webServer.BaseUrl + relativeUrl);
        }

        public void To(By clickDestination)
        {
            _executor.ActionOnLocator(clickDestination, e => e.Click());
        }
    }
}
