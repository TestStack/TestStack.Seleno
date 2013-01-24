using System;
using System.ComponentModel;
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
        private IWebDriver _browser;
        private readonly IScriptExecutor _execute;
        private readonly IElementFinder _finder;

        public PageReader(IWebDriver browser, IScriptExecutor executor, IElementFinder finder)
        {
            _browser = browser;
            _execute = executor;
            _finder = finder;
        }

        public TViewModel ModelFromPage()
        {
            var type = typeof(TViewModel);

            var instance = new TViewModel();
            foreach (var property in type.GetProperties())
            {
                var typedValue = _execute.ScriptAndReturn("$('#{0}').val()", property.PropertyType, property.Name);

                if (property.CanWriteToProperty(typedValue))
                {
                    property.SetValue(instance, typedValue, null);
                }
            }
            return instance;
        }

        public bool CheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(By.Name(name)).Selected;
        }

        public string TextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(By.Name(name)).GetAttribute("value");
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field)
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

        public TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName)
        {
            var element = ElementFor(propertySelector);
            var value = element.GetAttribute(attributeName) ?? string.Empty;
            return (TProperty)TypeDescriptor.GetConverter(typeof(TProperty)).ConvertFromString(value);
        }

        public TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector)
        {
            return GetAttributeAsType(propertySelector, "value");
        }

        public TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector)
        {
            string value = ElementFor(propertySelector).Text;
            return (TProperty)TypeDescriptor.GetConverter(typeof(TProperty)).ConvertFromString(value);
        }

    }
}