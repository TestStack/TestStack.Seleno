using OpenQA.Selenium;
using By = TestStack.Seleno.PageObjects.Locators.By;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IDropDown : ISelectableHtmlControl
    {
        string SelectedElementText { get; }
        void SelectElementByText(string optionText);
    }

    public class DropDown : SelectableHtmlControl, IDropDown
    {
        public string SelectedElementText { get { return SelectedElement.Text; } }

        public override IWebElement SelectedElement
        {
            get
            {
                var selector = string.Format("#{0} option:selected", Id);
                return Find().Element(By.jQuery(selector), WaitInSecondsUntilElementAvailable);
            }
        }

        // todo: unit test these methods

        public void SelectElementByText(string optionText)
        {
            var scriptToExecute = string.Format("$('#{0} option:contains(\"{1}\")').attr('selected',true)", Id, optionText.ToJavaScriptString());
            Execute().ExecuteScript(scriptToExecute);
        }

        public override void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = string.Format("$('#{0}').val('{1}')", Id, value.ToString().ToJavaScriptString());
            Execute().ExecuteScript(scriptToExecute);
        }
    }
}