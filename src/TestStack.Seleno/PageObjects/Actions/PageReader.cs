using System;
using System.Linq.Expressions;
using System.Web.Mvc;

using TestStack.Seleno.Extensions;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class PageReader<TViewModel> 
        where TViewModel : class ,new()
    {
        private RemoteWebDriver _browser;
        private readonly IScriptExecutor _execute;
        private readonly IElementFinder _finder;

        public PageReader(RemoteWebDriver browser, IScriptExecutor executor, IElementFinder finder)
        {
            _browser = browser;
            _execute = executor;
            _finder = finder;
        }

        public TViewModel ReadModelFromPage()
        {
            var type = typeof(TViewModel);

            var instance = new TViewModel();
            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var javascriptExtractor = string.Format("$('#{0}').val()", propertyName);
                var typedValue = _execute.ScriptAndReturn(javascriptExtractor, property.PropertyType);

                if (property.CanWriteToProperty(typedValue))
                {
                    property.SetValue(instance, typedValue, null);
                }
            }
            return instance;
        }

        public bool GetCheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(By.Name(name)).Selected;
        }

        public string GetTextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(By.Name(name)).GetAttribute("value");
        }

        public IWebElement GetElementFor<TField>(Expression<Func<TViewModel, TField>> field)
        {
            string name = ExpressionHelper.GetExpressionText(field);
            string id = TagBuilder.CreateSanitizedId(name);
            var element = _finder.TryFindElement(By.Id(id));
            return element;
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return _browser.ExecuteScriptAndReturn<bool>(javascriptExpression);
        }

    }
}