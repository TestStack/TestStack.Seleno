using System;
using System.Linq.Expressions;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects
{
    internal interface IComponentFactory
    {
        IPageReader<T> CreatePageReader<T>() where T : class, new();
        IPageWriter<T> CreatePageWriter<T>() where T : class, new();
        TPage CreatePage<TPage>() where TPage : UiComponent, new();

        THtmlControl HtmlControlFor<THtmlControl>(LambdaExpression propertySelector, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl;

        THtmlControl HtmlControlFor<THtmlControl>(string controlId, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl;
    }
}
