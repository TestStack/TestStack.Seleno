using System;
using System.Linq.Expressions;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    internal interface IComponentFactory
    {
        IPageReader<T> CreatePageReader<T>() where T : class, new();
        IPageWriter<T> CreatePageWriter<T>() where T : class, new();
        IElementAssert CreateElementAssert(By selector);
        TPage CreatePage<TPage>() where TPage : UiComponent, new();

        THtmlControl HtmlControlFor<THtmlControl>(LambdaExpression propertySelector, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl;

        THtmlControl HtmlControlFor<THtmlControl>(string controlId, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl;
    }
}
