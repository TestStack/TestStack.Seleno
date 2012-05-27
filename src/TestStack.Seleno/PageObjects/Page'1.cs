using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : Page
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

                if (propertyValue == null)
                    continue;

                var stringValue = GetStringValue(propertyTypeHandling, propertyValue, property);

                EnterTextInField(propertyName, stringValue);
            }
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