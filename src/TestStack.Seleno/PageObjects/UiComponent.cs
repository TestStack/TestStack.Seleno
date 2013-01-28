using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal readonly IWebDriver Browser;
        protected readonly IComponentFactory ComponentFactory;
        private readonly IPageNavigator _pageNavigator;
        private readonly IElementFinder _elementFinder;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly ICamera _camera;

        public UiComponent()
        {
            throw new InaccessibleSelenoException();
        }

        internal UiComponent(IWebDriver browser, IPageNavigator pageNavigator, IElementFinder elementFinder,
            IScriptExecutor scriptExecutor, ICamera camera, IComponentFactory componentFactory)
        {
            Browser = browser;
            ComponentFactory = componentFactory;
            _pageNavigator = pageNavigator;
            _elementFinder = elementFinder;
            _scriptExecutor = scriptExecutor;
            _camera = camera;
        }

        protected IPageNavigator Navigate()
        {
            return _pageNavigator;
        }

        protected IScriptExecutor Execute()
        {
            return _scriptExecutor;
        }

        protected IElementFinder Find()
        {
            return _elementFinder;
        }

        protected TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            return new TableReader<TModel>(gridId) { Browser = Browser };
        }

        public ElementAssert AssertThatElements(By selector)
        {
            return new ElementAssert(selector, _camera, Browser);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            return new TComponent();
        }
    }
}
