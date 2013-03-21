using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageWriter<TModel> : IPageWriter<TModel> where TModel : class, new()
    {
        private readonly IElementFinder _elementFinder;
        private readonly IComponentFactory _componentFactory;

        public PageWriter(IElementFinder elementFinder, IComponentFactory componentFactory)
        {
            _elementFinder = elementFinder;
            _componentFactory = componentFactory;
        }

        public void Model(TModel viewModel, IDictionary<Type, Func<object, string>> propertyTypeHandling = null)
        {
            var type = typeof(TModel);

            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(viewModel, null);

                if (property.GetCustomAttributes(typeof(HiddenInputAttribute), false).Length > 0)
                    continue;

                if (property.GetCustomAttributes(typeof(ScaffoldColumnAttribute), false).Length > 0)
                    continue;

                if (propertyValue == null)
                    continue;

                var stringValue = GetStringValue(propertyTypeHandling, propertyValue, property);

                var textBox = _componentFactory.HtmlControlFor<TextBox>(propertyName);
                textBox.ReplaceInputValueWith(stringValue);
            }
        }

        [Obsolete("Use ReplaceInputValueWith instead")]
        public void TextInField(string fieldName, string value)
        {
            ReplaceInputValueWith(fieldName,value);
        }

        protected string GetStringValue(IDictionary<Type, Func<object, string>> propertyTypeHandling, object propertyValue, PropertyInfo property)
        {
            if (propertyTypeHandling == null)
                return propertyValue.ToString();

            var propertyType = property.PropertyType;

            if (propertyTypeHandling.ContainsKey(propertyType))
            {
                var handler = propertyTypeHandling[propertyType];
                var propertyHandledValue = handler(propertyValue);

                return propertyHandledValue;
            }

            Type underlyingType;

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                underlyingType = propertyType.GetGenericArguments().Single();
            else
                return propertyValue.ToString();

            if (propertyTypeHandling.ContainsKey(underlyingType))
            {
                var handler = propertyTypeHandling[underlyingType];
                var propertyHandledValue = handler(propertyValue);

                return propertyHandledValue;
            }

            return propertyValue.ToString();
        }

        public void TickCheckbox(Expression<Func<TModel, bool>> propertySelector, bool isTicked)
        {
            var checkBox =_componentFactory.HtmlControlFor<CheckBox>(propertySelector);

            checkBox.Checked = isTicked;
        }


        public void ClearAndSendKeys<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, string value, bool clearFirst = true)
        {
            ClearAndSendKeys(ExpressionHelper.GetExpressionText(propertySelector), value, clearFirst);
        }

        public void ClearAndSendKeys(string elementName, string value, bool clearFirst = true)
        {
            var element = _elementFinder.Element(By.Name(elementName));
            if (clearFirst) element.Clear();
            element.SendKeys(value);
        }

        public void SetAttribute<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, String attributeName, TProperty attributeValue)
        {
            _componentFactory
                .HtmlControlFor<IHtmlControl>(propertySelector)
                .SetAttributeValue(attributeName, attributeValue);
        }

        public void ReplaceInputValueWith<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, TProperty inputValue)
        {
            _componentFactory
                .HtmlControlFor<TextBox>(propertySelector)
                .ReplaceInputValueWith(inputValue);
        }

        public void ReplaceInputValueWith(string inputName, string value)
        {
            _componentFactory
                .HtmlControlFor<TextBox>(inputName)
                .ReplaceInputValueWith(value);
        }

        public void SelectByOptionValueInDropDown<TProperty>(Expression<Func<TModel, TProperty>> dropDownSelector, TProperty optionValue)
        {
            _componentFactory
                .HtmlControlFor<DropDown>(dropDownSelector)
                .SelectElement(optionValue);
        }

        public void SelectByOptionTextInDropDown<TProperty>(Expression<Func<TModel, TProperty>> dropDownSelector, string optionText)
        {
            _componentFactory
               .HtmlControlFor<DropDown>(dropDownSelector)
               .SelectElementByText(optionText);
        }

        public void SelectButtonInRadioGroup<TProperty>(Expression<Func<TModel, TProperty>> radioGroupButtonSelector, TProperty buttonValue)
        {
            _componentFactory
                .HtmlControlFor<RadioButtonGroup>(radioGroupButtonSelector)
                .SelectElement(buttonValue);
        }

        public void UpdateTextAreaContent(Expression<Func<TModel, string>> textAreaPropertySelector, string content, TimeSpan maxWait = default(TimeSpan))
        {
            _componentFactory
                .HtmlControlFor<TextArea>(textAreaPropertySelector, maxWait)
                .Content = content;
        }
    }
}