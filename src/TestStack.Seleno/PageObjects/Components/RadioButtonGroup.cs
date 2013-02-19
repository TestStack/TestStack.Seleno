using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Components
{
    public class RadioButtonGroup : HTMLControl, ISelectableHtmlControl
    {
        private IWebElement SelectedRadioButtonInRadioGroup
        {
            get
            {
                var selector = string.Format("$('input[type=radio][name={0}]:checked')",Id);

                return Find().TryFindElement(By.jQuery(selector), WaitInSecondsUntilElementAvailable);
            }
        }

        public void SelectButtonInRadioGroup<TProperty>(TProperty buttonValue)
        {
            var scriptToExecute = string.Format("$('input[type=radio][name={0}][value={1}]').attr('checked',true)",
                                                Id,
                                                buttonValue);
            Execute().ExecuteScript(scriptToExecute);
        }

        public bool HasSelectedElement
        {
            get { return SelectedRadioButtonInRadioGroup != null; }
        }

        public TProperty SelectedElementAs<TProperty>()
        {
            var selectedRadioButtonElement = SelectedRadioButtonInRadioGroup;
            if (selectedRadioButtonElement == null)
            {
                throw new NoSuchElementException("No selected radio button has been found");
            }
            return selectedRadioButtonElement.GetControlValueAs<TProperty>();
        }

        public void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = string.Format("$('input[type=radio][name={0}][value={1}]').attr('checked',true)",
                                                Id,
                                                value);
            Execute().ExecuteScript(scriptToExecute);
        }
    }
}