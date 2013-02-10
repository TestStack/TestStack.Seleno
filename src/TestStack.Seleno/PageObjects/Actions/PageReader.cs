using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;


namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageReader<TViewModel> : IPageReader<TViewModel> where TViewModel : class ,new()
    {
        private readonly IWebDriver _browser;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IElementFinder _elementFinder;

        public PageReader(IWebDriver browser, IScriptExecutor scriptExecutor, IElementFinder elementFinder)
        {
            _browser = browser;
            _scriptExecutor = scriptExecutor;
            _elementFinder = elementFinder;
        }

        public TViewModel ModelFromPage()
        {
            var type = typeof(TViewModel);

            var instance = new TViewModel();
            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var javascriptExtractor = string.Format("$('#{0}').val()", propertyName);
                var typedValue = _scriptExecutor.ScriptAndReturn(javascriptExtractor, property.PropertyType);

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

            return _browser.FindElement(OpenQA.Selenium.By.Name(name)).Selected;
        }

        public string TextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(OpenQA.Selenium.By.Name(name)).GetAttribute("value");
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field)
        {
            string name = ExpressionHelper.GetExpressionText(field);
            string id = TagBuilder.CreateSanitizedId(name);
            var element = _elementFinder.TryFindElement(OpenQA.Selenium.By.Id(id));
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
            return 
                ElementFor(propertySelector)
                .GetAttributeAs<TProperty>(attributeName);
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

        public TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 0)
        {
            var dropDownId = ExpressionHelper.GetExpressionText(dropDownSelector);
            var selector = string.Format("$('#{0} option:selected')",dropDownId);
            var dropDownElement = _elementFinder.ElementWithWait(By.jQuery(selector), waitInSeconds);

            return dropDownElement.GetAttributeAs<TField>("value");
        }
    }
}