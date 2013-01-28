using Funq;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public class ComponentFactory : IComponentFactory
    {
        readonly Container _container;
        private readonly IWebDriver _browser;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IElementFinder _elementFinder;
        private readonly ICamera _camera;
        private readonly IPageNavigator _pageNavigator;

        public ComponentFactory(Container container)
        {
            _container = container;
            _browser = container.Resolve<IWebDriver>();
            _scriptExecutor = container.Resolve<IScriptExecutor>();
            _elementFinder = container.Resolve<IElementFinder>();
            _camera = container.Resolve<ICamera>();
            _pageNavigator = container.Resolve<IPageNavigator>();
        }

        public PageReader<TModel> CreatePageReader<TModel>() where TModel : class, new()
        {
            return new PageReader<TModel>(_browser, _scriptExecutor, _elementFinder);
        }

        public PageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            return new PageWriter<TModel>(_scriptExecutor, _elementFinder);
        }

        public ElementAssert CreateElementAssert(By selector)
        {
            return new ElementAssert(selector, _camera, _browser);
        }

        public TPage CreatePage<TPage>() where TPage : UiComponent, new()
        {
            return _container.Resolve<TPage>();
            //return (TPage)new UiComponent(_browser, _pageNavigator, _elementFinder, _scriptExecutor, _camera, this);
        }
    }
}