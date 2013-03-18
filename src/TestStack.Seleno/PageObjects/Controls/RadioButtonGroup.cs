using System.Web.Mvc;
using OpenQA.Selenium;
using By = TestStack.Seleno.PageObjects.Locators.By;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IRadioButtonGroup : ISelectableHtmlControl, IInputHtmlControl { }

    public class RadioButtonGroup : SelectableHtmlControl, IRadioButtonGroup
    {
        public override IWebElement SelectedElement
        {
            get
            {
                var selector = string.Format("input[type=radio][name={0}]:checked", Name);

                return Find().OptionalElement(By.jQuery(selector));
            }
        }

        public string Value { get { return ValueAs<string>(); }}

        public TReturn ValueAs<TReturn>()
        {
            return SelectedElementAs<TReturn>();
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            SelectElement(inputValue);
        }

        public override void SelectElement<TProperty>(TProperty value)
        {
            // todo: Is the .toLowerCase needed?
            var scriptToExecute = string.Format("$('input[type=radio][name={0}][value]')" +
                @".filter(function() {{return $(this).attr('value').toLowerCase() == ""{1}"".toLowerCase()}})" +
                ".attr('checked', true)",
                Name,
                value.ToString().ToJavaScriptString()
            );
            Execute().ExecuteScript(scriptToExecute);
        }
       
    }
}