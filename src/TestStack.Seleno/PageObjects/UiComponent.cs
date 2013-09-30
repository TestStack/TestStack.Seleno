using System;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Assertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal IWebDriver Browser { get; internal set; }
        internal IComponentFactory ComponentFactory { get; set; }
        internal IPageNavigator PageNavigator { get; set; }
        internal IExecutor Executor { get; set; }
        internal ICamera Camera { get; set; }
        internal IElementFinder ElementFinder { get; set; }
        internal IElementAssert ElementAssert { get; set; }
        internal IWait Wait { get; set; }

        public IPageNavigator Navigate
        {
            get
            {
                ThrowIfComponentNotCreatedCorrectly();
                return PageNavigator;
            }
        }

        public IExecutor Execute
        {
            get
            {
                ThrowIfComponentNotCreatedCorrectly();
                return Executor;
            }
        }

        public IElementFinder Find
        {
            get
            {
                ThrowIfComponentNotCreatedCorrectly();
                return ElementFinder;
            }
        }

        public TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return new TableReader<TModel>(gridId) {Browser = Browser};
        }

        public IElementAssert AssertThatElements
        {
            get
            {
                ThrowIfComponentNotCreatedCorrectly();
                return ElementAssert;
            }
        }

        public IWait WaitFor
        {
            get
            {
                ThrowIfComponentNotCreatedCorrectly();
                return Wait;
            }
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ComponentFactory.CreatePage<TComponent>();
        }

        public THtmlControl HtmlControlFor<THtmlControl>(string controlId, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : HTMLControl, new()
        {
            ThrowIfComponentNotCreatedCorrectly();
            return ComponentFactory.HtmlControlFor<THtmlControl>(controlId, maxWait);
        }

        private void ThrowIfComponentNotCreatedCorrectly()
        {
            if (PageNavigator == null)
                throw new InvalidOperationException(
                    "Don't new up page objects / components / controls; instead use SelenoHost.NavigateToInitialPage, Navigate(), HtmlControlFor() or GetComponent() as appropriate.");
        }
    }
}
