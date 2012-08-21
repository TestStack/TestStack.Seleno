using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class PageWriter<TModel> where TModel : class, new()
    {
        private RemoteWebDriver _browser;
        private readonly IScriptExecutor _execute;
        private readonly IElementFinder _finder;

        public PageWriter(RemoteWebDriver browser, IScriptExecutor executor, IElementFinder finder)
        {
            _browser = browser;
            _execute = executor;
            _finder = finder;
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

            _execute
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

    }
}