using System;
using System.Linq.Expressions;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel : class, new()
    {

        protected IPageReader<TModel> Read()
        {
            return ComponentFactory.CreatePageReader<TModel>();
        }

        protected IPageWriter<TModel> Input()
        {
            return ComponentFactory.CreatePageWriter<TModel>();
        }

        protected THtmlControl HtmlControlFor<THtmlControl>(Expression<Func<TModel, Object>> propertySelector, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : HTMLControl, new()
        {
            return ComponentFactory.HtmlControlFor<THtmlControl>(propertySelector, maxWait);
        }
    }
}