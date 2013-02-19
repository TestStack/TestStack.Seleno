using System;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Components
{
    public interface IDropDown : ISelectableHtmlControl
    {
        string SelectedElementText { get; }

        void SelectElementByText(string optionText);
    }

    public class DropDown : HTMLControl, IDropDown
    {

        private IWebElement SelectedOptionForDropDown
        {
            get
            {
                var selector = string.Format("$('#{0} option:selected')", Id);

                return Find().ElementWithWait(By.jQuery(selector), WaitInSecondsUntilElementAvailable);
            }
        }

        public bool HasSelectedElement
        {
            get { return SelectedOptionForDropDown != null; }
        }

        public string SelectedElementText
        {
            get { return SelectedOptionForDropDown.Text; }
        }

        
        public void SelectElementByText(string optionText)
        {
            var scriptToExecute = string.Format("$('#{0} option:contains(\"{1}\")').attr('selected',true)", Id, optionText);
            Execute().ExecuteScript(scriptToExecute);
        }

        public TProperty SelectedElementAs<TProperty>()
        {
            return SelectedOptionForDropDown.GetControlValueAs<TProperty>();
        }

        public void SelectElement<TProperty>(TProperty value)
        {
            var scriptToExecute = string.Format("$('#{0}').val('{1}')", Id, value);
            Execute().ExecuteScript(scriptToExecute);
        }

      

        
    }
}