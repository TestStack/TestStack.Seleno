using System;
using System.Linq.Expressions;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        internal protected IWebDriver Browser { get; internal set; }
        internal IComponentFactory ComponentFactory { get; set; }
        internal IPageNavigator PageNavigator { get; set; }
        internal IScriptExecutor ScriptExecutor { get; set; }
        internal ICamera Camera { get; set; }
        internal IElementFinder ElementFinder { get; set; }
        
        protected IPageNavigator Navigate()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return PageNavigator;
        }

        protected IScriptExecutor Execute()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ScriptExecutor;
        }

        protected IElementFinder Find()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ElementFinder;
        }

        protected TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return new TableReader<TModel>(gridId) { Browser = Browser };
        }

        protected ElementAssert AssertThatElements(By selector)
        {
            ThrowIfComponentNotCreatedCorrectly();
            return new ElementAssert(selector, Camera, Browser);
        }

        protected TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ComponentFactory.CreatePage<TComponent>();
        }

        protected THtmlControl HtmlControlFor<THtmlControl>(string controlId, int waitInSeconds = 20)
          where THtmlControl : HTMLControl, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ComponentFactory.HtmlControlFor<THtmlControl>(controlId, waitInSeconds);
        }

        private void ThrowIfComponentNotCreatedCorrectly()
        {
            if (PageNavigator == null)
                throw new InvalidOperationException("Don't new up Page Objects; instead use SelenoHost.NavigateToInitialPage");
        }
    }
}
