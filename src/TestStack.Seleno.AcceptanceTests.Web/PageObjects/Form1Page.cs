using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Seleno.AcceptanceTests.Web.PageObjects
{
    public class Form1Page : Page<Form1ViewModel>
    {
        public AssertionResultPage InputFixtureA()
        {
            Input().Model(Form1Fixtures.A);
            return Navigate().To<AssertionResultPage>(By.CssSelector("input[type=submit]"));
        }

        public Form1ViewModel ReadModel()
        {
            return Read().ModelFromPage();
        }

        public bool RequiredBoolCheckBoxIsTicked
        {
            get { return Read().CheckBoxValue(x => x.RequiredBool); }
        }

        public SomeEnum RequiredEnumDropDownSelectedValue
        {
            get { return Read().SelectedOptionValueInDropDown(x => x.RequiredEnum); }
        }

        public string RequiredEnumDropDownSelectedText
        {
            get { return Read().SelectedOptionTextInDropDown(x => x.RequiredEnum); }
        }

        public int RequiredIntTextBoxValue
        {
            get { return Read().GetValueFromTextBox(x => x.RequiredInt); }
        }

        public string[] TextAreaFieldContent
        {
            get { return Read().TextAreaContent(x => x.TextAreaField); }
        }

        public bool? OptionalBoolSelectedButtonValue
        {
            get { return Read().SelectedButtonInRadioGroup(x => x.OptionalBool); }
        }

        public bool OptionalListHasSelectedButton
        {
            get { return Read().HasSelectedRadioButtonInRadioGroup(x => x.OptionalListId); }
        }
    }
}