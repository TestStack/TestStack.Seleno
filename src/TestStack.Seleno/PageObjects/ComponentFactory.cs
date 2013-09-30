using System;
using System.Linq.Expressions;
using Autofac;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;

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
            return new PageReader<TModel>(_scope.Resolve<IExecutor>(), _scope.Resolve<IElementFinder>(), _scope.Resolve<IComponentFactory>());
        }

        public IPageWriter<TModel> CreatePageWriter<TModel>() where TModel : class, new()
        {
            return new PageWriter<TModel>(_scope.Resolve<IElementFinder>(), _scope.Resolve<IComponentFactory>());
        }

        public TPage CreatePage<TPage>() where TPage : UiComponent, new()
        {
            return _scope.Resolve<TPage>();
        }

        public THtmlControl HtmlControlFor<THtmlControl>(LambdaExpression propertySelector, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl
        {
            return  _scope.Resolve<THtmlControl>()
                .Initialize(propertySelector, maxWait);
        }

        public THtmlControl HtmlControlFor<THtmlControl>(string controlId, TimeSpan maxWait = default(TimeSpan))
           where THtmlControl : IHtmlControl
        {
            return _scope.Resolve<THtmlControl>()
                .Initialize(controlId, maxWait);
        }
    }
}