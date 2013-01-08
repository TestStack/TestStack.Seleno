using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal readonly RemoteWebDriver Browser;
        private readonly PageNavigator _navigator;
        protected readonly ElementFinder ElementFinder;
        protected readonly ScriptExecutor ScriptExecutor;
        readonly ICamera _camera;

        public UiComponent()
        {
            if (SelenoApplicationRunner.Host == null)
                throw new AppConfigurationException("SelenoApplicationRunner.Host is not set");
            Browser = SelenoApplicationRunner.Host.Browser as RemoteWebDriver;
            _camera = SelenoApplicationRunner.Host.Camera;
            ElementFinder = new ElementFinder(Browser);
            ScriptExecutor = new ScriptExecutor(Browser, ElementFinder, _camera);
            _navigator = new PageNavigator(Browser, ScriptExecutor);
        }

        protected IPageNavigator Navigate()
        {
            return _navigator;
        }

        protected IScriptExecutor Execute()
        {
            return ScriptExecutor;
        }

        protected IElementFinder Find()
        {
            return ElementFinder;
        }

        protected TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            return new TableReader<TModel>(gridId) { Browser = Browser };
        }

        public ElementAssert AssertThatElements(By selector)
        {
            return new ElementAssert(selector, _camera);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            return new TComponent();
        }
    }
}
