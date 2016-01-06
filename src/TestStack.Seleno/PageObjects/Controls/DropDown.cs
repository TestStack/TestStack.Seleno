using OpenQA.Selenium;
using By = TestStack.Seleno.PageObjects.Locators.By;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class DropDown : SelectableHtmlControl
    {
        public virtual string SelectedElementText { get { return SelectedElement.Text; } }

        public override IWebElement SelectedElement
        {
            get
            {
                var selector = string.Format("#{0} option:selected", Id);
                return Find.Element(By.jQuery(selector), WaitUntilElementAvailable);
            }
        }

        // todo: unit test these methods

        public virtual void SelectElementByText(string optionText)
        {
            var scriptToExecute = string.Format("$('#{0} option:contains(\"{1}\")').attr('selected',true).change()", Id, optionText.ToJavaScriptString());
            Execute.Script(scriptToExecute);
        }

        public override void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = string.Format("$('#{0}').val('{1}')", Id, value.ToString().ToJavaScriptString());
            Execute.Script(scriptToExecute);
        }
    }
}