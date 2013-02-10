using Autofac;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    internal class ComponentFactory : IComponentFactory
    {
        private readonly ILifetimeScope _scope;

        public ComponentFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IPageReader<TModel> CreatePageReader<TModel>() where TModel : class, new()
        {
            return new PageReader<TModel>(_scope.Resolve<IWebDriver>(), _scope.Resolve<IScriptExecutor>(), _scope.Resolve<IElementFinder>());
        }

        public IPageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            return new PageWriter<TModel>(_scope.Resolve<IScriptExecutor>(), _scope.Resolve<IElementFinder>());
        }

        public IElementAssert CreateElementAssert(By selector)
        {
            return new ElementAssert(selector, _scope.Resolve<ICamera>(), _scope.Resolve<IWebDriver>());
        }

        public TPage CreatePage<TPage>() where TPage : UiComponent, new()
        {
            return _scope.Resolve<TPage>();
        }
    }
}