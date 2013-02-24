using System;
using System.Linq.Expressions;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Specifications.Assertions;

namespace TestStack.Seleno.PageObjects
{
    public interface IComponentFactory
    {
        IPageReader<T> CreatePageReader<T>() where T : class, new();
        IPageWriter<T> CreatePageWriter<T>() where T : class, new();
        IElementAssert CreateElementAssert(By selector);
        TPage CreatePage<TPage>() where TPage : UiComponent, new();

        THtmlControl HtmlControlFor<THtmlControl>(LambdaExpression propertySelector, int waitInSeconds = 20)
            where THtmlControl : IHTMLControl;

        THtmlControl HtmlControlFor<THtmlControl>(string controlId, int waitInSeconds = 20)
            where THtmlControl : IHTMLControl;


    }
}
