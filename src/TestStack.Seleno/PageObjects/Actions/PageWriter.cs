using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class PageWriter<TModel> where TModel : class, new()
    {
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IElementFinder _elementFinder;

        public PageWriter(IScriptExecutor scriptExecutor, IElementFinder elementFinder)
        {
            _scriptExecutor = scriptExecutor;
            _elementFinder = elementFinder;
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

                TextInField(propertyName, stringValue);
            }
        }

        public void TextInField(string fieldName, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _scriptExecutor
                .ActionOnLocator(
                    By.Name(fieldName),
                    element =>
                    {
                        element.Clear();
                        if (!string.IsNullOrEmpty(value))
                            element.SendKeys(value);
                    });
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

        public void ClearAndSendKeys<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, string value, bool clearFirst = true)
        {
            ClearAndSendKeys(ExpressionHelper.GetExpressionText(propertySelector), value, clearFirst);
        }

        public void ClearAndSendKeys(string elementName, string value, bool clearFirst = true)
        {
            var element = _elementFinder.ElementWithWait(By.Name(elementName));
            if (clearFirst) element.Clear();
            element.SendKeys(value);
        }

        public void SetAttribute<TProperty>(Expression<Func<TModel, TProperty>> propertySelector,String attributeName, TProperty attributeValue)
        {
            var name = ExpressionHelper.GetExpressionText(propertySelector);

            var scriptToExecute = string.Format("$('#{0}').attr('{1}','{2}'))", name, attributeName, attributeValue);

            _scriptExecutor.ExecuteScript(scriptToExecute);
        }

        public void ReplaceInputValueWith<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, TProperty inputValue)
        {

            var name = ExpressionHelper.GetExpressionText(propertySelector);
            var scriptToExecute = string.Format("$('{0}').val('{1}')", name, inputValue);
            _scriptExecutor.ExecuteScript(scriptToExecute);
        }
    }
}