using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void Input(object o, ParameterExpression parentParameter, LambdaExpression expression, IDictionary<Type, Func<object, string>> propertyTypeHandling, 
            List<string> membersBlacklist = null)
        {
            var type = o.GetType();

            if (membersBlacklist == null)
                membersBlacklist = new List<string>();

            foreach (var property in type.GetProperties().Where(x => !membersBlacklist.Exists(y => y == x.Name)))
                InputProperty(o, parentParameter, expression, propertyTypeHandling, property);
        }

        private void InputProperty(object o, ParameterExpression parentParameter, LambdaExpression expression,
            IDictionary<Type, Func<object, string>> propertyTypeHandling, PropertyInfo property)
        {
            var customAttributes = property.GetCustomAttributes(false);

            if (customAttributes.OfType<HiddenInputAttribute>().Any())
                return;

            if (customAttributes.OfType<ScaffoldColumnAttribute>().Any(x => !x.Scaffold))
                return;

            if (customAttributes.OfType<ReadOnlyAttribute>().Any(x => x.IsReadOnly))
                return;

            var propertyValue = property.GetValue(o, null);
            if (propertyValue == null)
                return;

            var p = Expression.Property(expression != null ? expression.Body : parentParameter, property);
            var propertyExpression = Expression.Lambda(p, parentParameter);

            if (!property.PropertyType.IsValueType && property.PropertyType != typeof (string))
            {
                Input(propertyValue, parentParameter, propertyExpression, propertyTypeHandling);
                return;
            }

            var stringValue = GetStringValue(propertyTypeHandling, propertyValue, property.PropertyType);

			var ctrl = _componentFactory.HtmlControlFor<TextBox>(propertyExpression);
            ctrl.ReplaceInputValueWith(stringValue);
        }

        public void Model(TModel viewModel, IDictionary<Type, Func<object, string>> propertyTypeHandling = null, params string[] propertyBlacklist)
        {
            Input(viewModel, Expression.Parameter(viewModel.GetType(), "m"), null, propertyTypeHandling, propertyBlacklist.ToList());
        }


        public void Field<T>(Expression<Func<TModel, T>> field, T value, IDictionary<Type, Func<object, string>> propertyTypeHandling = null)
        {
            var param = field.Parameters.First();
            if (!typeof(T).IsValueType && typeof(T) != typeof(string))
                Input(value, param, field, propertyTypeHandling);

            var stringValue = GetStringValue(propertyTypeHandling, value, typeof(T));
            _componentFactory.HtmlControlFor<TextBox>(field)
                .ReplaceInputValueWith(stringValue);
        }

        [Obsolete("Use ReplaceInputValueWith instead")]
        public void TextInField(string fieldName, string value)
        {
            ReplaceInputValueWith(fieldName,value);
        }

        protected string GetStringValue(IDictionary<Type, Func<object, string>> propertyTypeHandling, object propertyValue, Type propertyType)
        {
            if (propertyTypeHandling == null)
                return propertyValue.ToString();

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