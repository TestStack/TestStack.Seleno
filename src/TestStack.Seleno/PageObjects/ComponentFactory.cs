using System;
using Funq;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public class ComponentFactory : IComponentFactory
    {
        private readonly Func<IWebDriver> _browser;
        private readonly Func<IScriptExecutor> _scriptExecutor;
        private readonly Func<IElementFinder> _elementFinder;
        private readonly Func<ICamera> _camera;
        private readonly Func<IPageNavigator> _pageNavigator;

        public ComponentFactory(Func<IWebDriver> browser, Func<IScriptExecutor> scriptExecutor, 
            Func<IElementFinder> elementFinder, Func<ICamera> camera, Func<IPageNavigator> pageNavigator)
        {
            _browser = browser;
            _scriptExecutor = scriptExecutor;
            _elementFinder = elementFinder;
            _camera = camera;
            _pageNavigator = pageNavigator;
        }

        public IPageReader<TModel> CreatePageReader<TModel>() where TModel : class, new()
        {
            return new PageReader<TModel>(_browser(), _scriptExecutor(), _elementFinder());
        }

        public IPageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            return new PageWriter<TModel>(_scriptExecutor(), _elementFinder());
        }

        public IElementAssert CreateElementAssert(By selector)
        {
            return new ElementAssert(selector, _camera(), _browser());
        }

        public TPage CreatePage<TPage>() where TPage : UiComponent, new()
        {
            var page = new TPage();

            page.Setup(_browser(), _pageNavigator(), _elementFinder(), _scriptExecutor(), _camera(), this);

            return page;
        }
    }
}