using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.PageObjects.Components
{
    internal class FormComponent<TViewModel> where TViewModel : class, new()
    {
        protected internal RemoteWebDriver Browser = null;

        //public void FillWithModel(TViewModel viewModel, IDictionary<Type, Func<object, string>> propertyTypeHandling = null)
        //{
        //    var type = typeof(TViewModel);
        //    foreach (var property in type.GetProperties())
        //    {
        //        var propertyName = property.Name;
        //        var propertyValue = property.GetValue(viewModel, null);

        //        if (property.GetCustomAttributes(typeof(HiddenInputAttribute), false).Length > 0)
        //            continue;

        //        if (property.GetCustomAttributes(typeof(ScaffoldColumnAttribute), false).Length > 0)
        //            continue;

        //        if (propertyValue == null)
        //            continue;

        //        var stringValue = GetStringValue(propertyTypeHandling, propertyValue, property);

        //        EnterTextInField(propertyName, stringValue);
        //    }
        //}

        //private void EnterTextInField(string fieldName, string value)
        //{
        //    if (String.IsNullOrEmpty(value)) return;

        //    Execute(By.Name(fieldName),
        //            element =>
        //            {
        //                element.Clear();
        //                if (!string.IsNullOrEmpty(value))
        //                    element.SendKeys(value);
        //            });
        //}

    }
}