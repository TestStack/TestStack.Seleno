using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TViewModel> : UiComponent
        where TViewModel: new()
    {
        public bool GetCheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return Browser.FindElement(By.Name(name)).Selected;
        }

        public string GetTextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return Browser.FindElement(By.Name(name)).GetAttribute("value");
        }

        public void FillWithModel(TViewModel viewModel, IDictionary<Type, Func<object, string>> propertyTypeHandling = null)
        {
            var type = typeof(TViewModel);
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

                EnterTextInField(propertyName, stringValue);
            }
        }

        public TViewModel ReadModelFromPage()
        {
            var type = typeof(TViewModel);

            var instance = new TViewModel();
            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var javascriptExtractor = string.Format("$('#{0}').val()", propertyName);
                var typedValue = ExecuteScriptAndReturn(javascriptExtractor, property.PropertyType);
                
                if (CanWriteProperty(typedValue, property))
                {
                    property.SetValue(instance, typedValue, null);
                }
            }
            return instance;
        }

       
        private bool CanWriteProperty(Object typedValue, PropertyInfo property)
        {
            return typedValue != null && property != null && property.CanWrite;
        }

        public IWebElement GetElementFor<TField>(Expression<Func<TViewModel, TField>> field)
        {
            string name = ExpressionHelper.GetExpressionText(field);
            string id = TagBuilder.CreateSanitizedId(name);
            var element = TryFindElement(By.Id(id));
            return element;
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return Browser.ExecuteScriptAndReturn<bool>(javascriptExpression);
        }

        private string GetStringValue(IDictionary<Type, Func<object, string>> propertyTypeHandling, object propertyValue, PropertyInfo property)
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

        private void EnterTextInField(string fieldName, string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            Execute(By.Name(fieldName), element =>
                                            {
                                                element.Clear();
                                                if (!string.IsNullOrEmpty(value))
                                                    element.SendKeys(value);
                                            });
        }
    }
}