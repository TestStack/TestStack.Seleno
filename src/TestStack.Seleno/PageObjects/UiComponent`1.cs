using System;
using System.Linq.Expressions;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel : class, new()
    {
        public IPageReader<TModel> Read
        {
            get
            {
                return ComponentFactory.CreatePageReader<TModel>();
            }
        }

        public IPageWriter<TModel> Input
        {
            get
            {
                return ComponentFactory.CreatePageWriter<TModel>();
            }
        }

        public THtmlControl HtmlControlFor<THtmlControl>(Expression<Func<TModel, Object>> propertySelector,
            TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : HTMLControl, new()
        {
            return ComponentFactory.HtmlControlFor<THtmlControl>(propertySelector, maxWait);
        }
    }
}