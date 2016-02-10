using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class DropDown : SelectableHtmlControl
    {
        public virtual string SelectedElementText => SelectedElement.Text;

        public override IWebElement SelectedElement
        {
            get
            {
                var selector = $"#{Id} option:selected";
                return Find.Element(By.jQuery(selector), WaitUntilElementAvailable);
            }
        }

        // todo: unit test these methods

        public virtual void SelectElementByText(string optionText)
        {
            var scriptToExecute =
                $"$('#{Id} option:contains(\"{optionText.ToJavaScriptString()}\")').attr('selected',true).change()";
            Execute.Script(scriptToExecute);
        }

        public override void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = $"$('#{Id}').val('{value.ToString().ToJavaScriptString()}')";
            Execute.Script(scriptToExecute);
        }
    }
}