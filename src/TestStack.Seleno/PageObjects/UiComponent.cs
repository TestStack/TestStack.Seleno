using System;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal IWebDriver Browser;
        internal IComponentFactory ComponentFactory;
        internal IPageNavigator PageNavigator;
        internal IElementFinder ElementFinder;
        internal IScriptExecutor ScriptExecutor;
        internal ICamera Camera;

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

        public ElementAssert AssertThatElements(By selector)
        {
            ThrowIfComponentNotCreatedCorrectly();
            return new ElementAssert(selector, Camera, Browser);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ComponentFactory.CreatePage<TComponent>();
        }

        private void ThrowIfComponentNotCreatedCorrectly()
        {
            if (PageNavigator == null)
                throw new InvalidOperationException("Don't new up Page Objects; instead use SelenoHost.NavigateToInitialPage");
        }
    }
}
