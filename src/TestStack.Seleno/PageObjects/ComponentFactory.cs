using System;
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
            where THtmlControl : IHtmlControl
        {
            return GetHtmlControlFor<THtmlControl>(c => c.ViewModelPropertySelector = propertySelector, waitInSeconds);
        }

        public THtmlControl HtmlControlFor<THtmlControl>(string  controlId, int waitInSeconds = 20)
           where THtmlControl : IHtmlControl
        {
            return GetHtmlControlFor<THtmlControl>(c => c.Id = controlId, waitInSeconds);
        }


        private THtmlControl GetHtmlControlFor<THtmlControl>(Action<HTMLControl> htmlControlInitialiser, int waitInSecondsUntilElementAvailable = 20) 
            where THtmlControl : IHtmlControl
        {
            
            var htmlControl = _scope.Resolve<THtmlControl>();
            var result = htmlControl as HTMLControl;

            if (result == null) throw new SelenoException(string.Format("{0} should derive from HTMLControl",typeof(THtmlControl).Name));

            htmlControlInitialiser(result);
            result.WaitInSecondsUntilElementAvailable = waitInSecondsUntilElementAvailable;

            result.Browser = _scope.Resolve<IWebDriver>();
            result.Camera = _scope.Resolve<ICamera>();
            result.ComponentFactory = _scope.Resolve<IComponentFactory>();
            result.ElementFinder = _scope.Resolve<IElementFinder>();
            result.PageNavigator = _scope.Resolve<IPageNavigator>();
            result.ScriptExecutor = _scope.Resolve<IScriptExecutor>();    

            return (THtmlControl)(IHtmlControl)result;
        }
       
    }
}