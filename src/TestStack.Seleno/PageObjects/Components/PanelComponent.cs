using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Components
{
    internal class PanelComponent<TViewModel> 
        where TViewModel : class ,new()
    {
        protected internal RemoteWebDriver Browser = null;

        public bool GetCheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return Browser.FindElement(OpenQA.Selenium.By.Name(name)).Selected;
        }

        public string GetTextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return Browser.FindElement(OpenQA.Selenium.By.Name(name)).GetAttribute("value");
        }

        //public TViewModel ReadModelFromPage()
        //{
        //    var type = typeof(TViewModel);

        //    var instance = new TViewModel();
        //    foreach (var property in type.GetProperties())
        //    {
        //        var propertyName = property.Name;
        //        var javascriptExtractor = string.Format("$('#{0}').val()", propertyName);
        //        var typedValue = Browser.ExecuteScriptAndReturn(javascriptExtractor, property.PropertyType);

        //        if (property.CanWriteToProperty(typedValue))
        //        {
        //            property.SetValue(instance, typedValue, null);
        //        }
        //    }
        //    return instance;
        //}

        //public IWebElement GetElementFor<TField>(Expression<Func<TViewModel, TField>> field)
        //{
        //    string name = ExpressionHelper.GetExpressionText(field);
        //    string id = TagBuilder.CreateSanitizedId(name);
        //    var element = TryFindElement(By.Id(id));
        //    return element;
        //}

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return Browser.ExecuteScriptAndReturn<bool>(javascriptExpression);
        }
    }
}