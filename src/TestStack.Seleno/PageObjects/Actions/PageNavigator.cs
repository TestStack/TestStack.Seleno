using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Fakes;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class PageNavigator : IPageNavigator
    {
        protected IWebDriver Browser;
        readonly IScriptExecutor _executor;
        private readonly IWebServer _webServer;

        internal PageNavigator(IWebDriver browser, IScriptExecutor executor, IWebServer webServer)
        {
            Browser = browser;
            _executor = executor;
            _webServer = webServer;
        }

        public TPage To<TPage>(By clickDestination) where TPage : UiComponent, new()
        {
            To(clickDestination);
            return new TPage();
        }

        public TPage To<TPage>(string url) where TPage : UiComponent, new()
        {
            Browser.Navigate().GoToUrl(url);
            return new TPage();
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
