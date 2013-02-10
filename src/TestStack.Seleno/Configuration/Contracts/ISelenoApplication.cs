using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface ISelenoApplication
    {
        void Initialize();
        void ShutDown();
        IWebDriver Browser { get; }
        IWebServer WebServer { get; }
        ICamera Camera { get; }

        TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new();

        TPage NavigateToInitialPage<TPage>(string relativeUrl = "") where TPage : UiComponent, new();
    }
}
