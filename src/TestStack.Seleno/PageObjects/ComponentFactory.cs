using System.Linq.Expressions;
using Autofac;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
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
            return new PageReader<TModel>(_scope.Resolve<IScriptExecutor>(), _scope.Resolve<IElementFinder>(), _scope.Resolve<IComponentFactory>());
        }

        public IPageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            return new PageWriter<TModel>(_scope.Resolve<IScriptExecutor>(), _scope.Resolve<IElementFinder>(), _scope.Resolve<IComponentFactory>());
        }

        public IElementAssert CreateElementAssert(By selector)
        {
            return new ElementAssert(selector, _scope.Resolve<ICamera>(), _scope.Resolve<IWebDriver>());
        }

        public TPage CreatePage<TPage>() where TPage : UiComponent, new()
        {
            return _scope.Resolve<TPage>();
        }

        public THtmlControl HtmlControlFor<THtmlControl>(LambdaExpression propertySelector, int waitInSeconds = 20)
            where THtmlControl : IHtmlControl
        {
            return  _scope.Resolve<THtmlControl>()
                .Initialize(propertySelector, waitInSeconds);
        }

        public THtmlControl HtmlControlFor<THtmlControl>(string  controlId, int waitInSeconds = 20)
           where THtmlControl : IHtmlControl
        {
            return _scope.Resolve<THtmlControl>()
                .Initialize(controlId, waitInSeconds);
        }
    }
}