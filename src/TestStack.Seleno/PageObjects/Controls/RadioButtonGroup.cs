using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class RadioButtonGroup : SelectableHtmlControl, IInputHtmlControl
    {
        protected override IWebElement SelectedElement
        {
            get
            {
                var selector = string.Format("$('input[type=radio][name={0}]:checked')", Name);

                return Find().TryFindElement(By.jQuery(selector), WaitInSecondsUntilElementAvailable);
            }
        }

        public TReturn ValueAs<TReturn>()
        {
            return SelectedElementAs<TReturn>();
        }

        public InputType Type
        {
            get { return InputType.Radio; }
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            SelectElement(inputValue);
        }

        public override void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = string.Format("$('input[type=radio][name={0}][value={1}]').attr('checked',true)",
                                                Name,
                                                value);
            Execute().ExecuteScript(scriptToExecute);
        }
       
    }
}