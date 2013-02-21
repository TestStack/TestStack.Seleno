using System.Linq.Expressions;
using Autofac;
using Autofac.Core;
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
            return new PageReader<TModel>(_scope.Resolve<IWebDriver>(), _scope.Resolve<IScriptExecutor>(), _scope.Resolve<IElementFinder>(), _scope.Resolve<IComponentFactory>());
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
            where THtmlControl : HTMLControl, new()
        {
            var htmlControl = _scope.Resolve<THtmlControl>();
            htmlControl.ViewModelPropertySelector = propertySelector;
            htmlControl.WaitInSecondsUntilElementAvailable = waitInSeconds;

            return htmlControl;
        }

        public THtmlControl HtmlControlFor<THtmlControl>(string  controlId, int waitInSeconds = 20)
           where THtmlControl : HTMLControl, new()
        {
            var htmlControl = _scope.Resolve<THtmlControl>();
            htmlControl.Id = controlId;
            htmlControl.WaitInSecondsUntilElementAvailable = waitInSeconds;

            return htmlControl;
        }
    }
}