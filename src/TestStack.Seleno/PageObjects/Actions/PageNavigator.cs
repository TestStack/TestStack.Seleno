using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Fakes;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class PageNavigator : IPageNavigator
    {
        protected RemoteWebDriver Browser;
        readonly IScriptExecutor _executor;

        internal PageNavigator(RemoteWebDriver browser, IScriptExecutor executor)
        {
            Browser = browser;
            _executor = executor;
        }

        public TPage To<TPage>(By clickDestination) where TPage : UiComponent, new()
        {
            To(clickDestination);
            return new TPage();
        }

        public TPage To<TPage>(string relativeUrl) where TPage : UiComponent, new()
        {
            Browser.Navigate().GoToUrl(relativeUrl);
            return new TPage();
        }

        public TPage To<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            var helper = new HtmlHelper(new ViewContext { HttpContext = FakeHttpContext.Root() }, new FakeViewDataContainer());
            var relativeUrl = helper.BuildUrlFromExpression(action);

            return To<TPage>(IISExpressRunner.HomePage + relativeUrl);
        }

        public void To(By clickDestination)
        {
            _executor.ActionOnLocator(clickDestination, e => e.Click());
        }
    }
}
