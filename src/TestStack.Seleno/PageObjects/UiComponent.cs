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
        private bool _isSetup;

        protected internal IWebDriver Browser;
        // todo: Should this be internal?
        protected IComponentFactory ComponentFactory;
        private IPageNavigator _pageNavigator;
        private IElementFinder _elementFinder;
        private IScriptExecutor _scriptExecutor;
        private ICamera _camera;

        internal void Setup(IWebDriver browser, IPageNavigator pageNavigator, IElementFinder elementFinder,
            IScriptExecutor scriptExecutor, ICamera camera, IComponentFactory componentFactory)
        {
            _isSetup = true;
            Browser = browser;
            ComponentFactory = componentFactory;
            _pageNavigator = pageNavigator;
            _elementFinder = elementFinder;
            _scriptExecutor = scriptExecutor;
            _camera = camera;
        }

        private void EnsureSetup()
        {
            if (!_isSetup)
                throw new InvalidOperationException("This property cannot be used since the Setup() method hasn't been called on the page object / component. The likely reason for this is that the page object was explicitly created rather than creating via a Seleno Navigate method or GetComponent method.");
        }

        protected IPageNavigator Navigate()
        {
            EnsureSetup();
            return _pageNavigator;
        }

        protected IScriptExecutor Execute()
        {
            EnsureSetup();
            return _scriptExecutor;
        }

        protected IElementFinder Find()
        {
            EnsureSetup();
            return _elementFinder;
        }

        protected TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            EnsureSetup();
            return new TableReader<TModel>(gridId) { Browser = Browser };
        }

        public ElementAssert AssertThatElements(By selector)
        {
            EnsureSetup();
            return new ElementAssert(selector, _camera, Browser);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            var component = new TComponent();

            component.Setup(Browser, _pageNavigator, _elementFinder, _scriptExecutor, _camera, ComponentFactory);

            return component;
        }
    }
}
