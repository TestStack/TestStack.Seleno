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
            set { Input().TickCheckbox(x => x.RequiredBool, value); }
        }

        public SomeEnum RequiredEnumDropDownSelectedValue
        {
            get { return Read().SelectedOptionValueInDropDown(x => x.RequiredEnum); }
            set { Input().SelectByOptionValueInDropDown(x => x.RequiredEnum,value); }
        }

        public string RequiredEnumDropDownSelectedText
        {
            get { return Read().SelectedOptionTextInDropDown(x => x.RequiredEnum); }
            set { Input().SelectByOptionTextInDropDown(x => x.RequiredEnum, value); }
        }

        public int RequiredIntTextBoxValue
        {
            get { return Read().GetValueFromTextBox(x => x.RequiredInt); }
            set { Input().ReplaceInputValueWith(x=> x.RequiredInt,value); }
        }

        public string[] TextAreaFieldContent
        {
            get { return Read().TextAreaContent(x => x.TextAreaField); }
            set {Input().UpdateTextAreaContent(x => x.TextAreaField,value); }
        }

        public bool? OptionalBoolSelectedButtonValue
        {
            get { return Read().SelectedButtonInRadioGroup(x => x.OptionalBool); }
            set { Input().SelectButtonInRadioGroup(x => x.OptionalBool,value); }
        }

        public bool OptionalListHasSelectedButton
        {
            get { return Read().HasSelectedRadioButtonInRadioGroup(x => x.OptionalListId); }
        }
    }
}