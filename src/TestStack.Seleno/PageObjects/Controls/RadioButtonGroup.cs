using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class RadioButtonGroup : SelectableHtmlControl
    {
        public override IWebElement SelectedElement
        {
            get
            {
                var selector = $"input[type=radio][name='{Name}']:checked";

                return Find.OptionalElement(By.jQuery(selector));
            }
        }

        public string Value => ValueAs<string>();

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
            var scriptToExecute = $"$(\"input[type=radio][name='{Name}'][value]\")" +
                                  $@".filter(function() {{return $(this).attr('value').toLowerCase() == ""{value
                                      .ToString().ToJavaScriptString()}"".toLowerCase()}})" + ".attr('checked', true)";
            Execute.Script(scriptToExecute);
        }
       
    }
}