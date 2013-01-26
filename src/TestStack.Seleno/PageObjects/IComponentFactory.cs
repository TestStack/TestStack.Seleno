using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    // In a class based approach you would have a SUT. There are some things that just belong here and NavigateToInitialPage is one.
    // This is probably the SelenoApplication so might just be a case of exposing that to users of Seleno
    public static class SUT
    {
        public static TDestinationPage NavigateToInitialPage<TDestinationPage, TController>(Expression<Action<TController>> action)
            where TController : Controller
            where TDestinationPage : UiComponent, new()
        {
            // return NavigateToPage<TDestinationPage, TController>(action, Browser);
            throw new NotImplementedException();
        }

        private static TDestinationPage NavigateToPage<TDestinationPage, TController>(Expression<Action<TController>> action,
            IWebDriver browser)
            where TController : Controller
            where TDestinationPage : UiComponent, new()
        {
            //var helper = new HtmlHelper(new ViewContext { HttpContext = FakeHttpContext.Root() }, new FakeViewDataContainer());
            //var relativeUrl = helper.BuildUrlFromExpression(action);

            //string normalisedUrl = string.Format("{0}{1}", BaseUrl, relativeUrl);
            //browser.Navigate().GoToUrl(normalisedUrl);
            //WaitFor.AjaxCallsToComplete(browser);

            //return new TDestinationPage { Browser = browser };
            throw new NotImplementedException();
        }
    }

    public interface IComponentFactory
    {
        PageReader<T> CreatePageReader<T>() where T : class, new();
        PageWriter<T> CreatePageWriter<T>() where T : class, new();

        ElementAssert CreateElementAssert(By selector);

        T CreatePage<T>() where T : class, new();
    }

    public class ComponentFactory : IComponentFactory
    {
        public PageReader<TModel> CreatePageReader<TModel>() where TModel : class, new()
        {
            throw new System.NotImplementedException();
        }

        public PageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            throw new System.NotImplementedException();
        }

        public ElementAssert CreateElementAssert(By selector)
        {
            throw new System.NotImplementedException();
        }

        public T CreatePage<T>() where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
