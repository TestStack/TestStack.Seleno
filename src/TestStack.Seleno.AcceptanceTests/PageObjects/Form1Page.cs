using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public class Form1Page : Page<Form1ViewModel>
    {
        public AssertionResultPage InputFixtureA()
        {
            Input.Model(Form1Fixtures.A);
            return Navigate.To<AssertionResultPage>(By.TagName("button"));
        }

        public Form1ViewModel ReadModel()
        {
            return Read.ModelFromPage();
        }

        public bool RequiredBoolCheckBoxIsTicked
        {
            get { return Read.CheckBoxValue(x => x.RequiredBool); }
            set { Input.TickCheckbox(x => x.RequiredBool, value); }
        }

        public SomeEnum RequiredEnumDropDownSelectedValue
        {
            get { return Read.SelectedOptionValueInDropDown(x => x.RequiredEnum); }
            set { Input.SelectByOptionValueInDropDown(x => x.RequiredEnum, value); }
        }

        public string RequiredEnumDropDownSelectedText
        {
            get { return Read.SelectedOptionTextInDropDown(x => x.RequiredEnum); }
            set { Input.SelectByOptionTextInDropDown(x => x.RequiredEnum, value); }
        }

        public int RequiredIntTextBoxValue
        {
            get { return Read.GetValueFromTextBox(x => x.RequiredInt); }
            set { Input.ReplaceInputValueWith(x=> x.RequiredInt, value); }
        }

        public string TextAreaFieldContent
        {
            get { return Read.TextAreaContent(x => x.TextAreaField); }
            set { Input.UpdateTextAreaContent(x => x.TextAreaField, value); }
        }

        public bool? OptionalBoolAsListSelectedButtonValue
        {
            get { return Read.SelectedButtonInRadioGroup(x => x.OptionalBoolAsList); }
            set { Input.SelectButtonInRadioGroup(x => x.OptionalBoolAsList, value); }
        }

        public bool RequiredListHasSelectedButton
        {
            get { return Read.HasSelectedRadioButtonInRadioGroup(x => x.RequiredListIdAsList); }
        }

        public IWebElement FindExistentElement
        {
            get { return Find.Element(By.Id("RequiredBool")); }
        }

        public void AssertElementExists(string id)
        {
            AssertThatElements.Exist(By.Id(id));
        }

        public void AssertElementDoesNotExist(string id)
        {
            AssertThatElements.DoNotExist(By.Id(id));
        }

        public void AssertElementDoesNotExistWithJquery(string id)
        {
            AssertThatElements.DoNotExist(Seleno.PageObjects.Locators.By.jQuery(string.Format("#{0}", id)));
        }

        public void AssertElementExistsWithJQuery(string id)
        {
            AssertThatElements.Exist(Seleno.PageObjects.Locators.By.jQuery(string.Format("#{0}", id)));
        }

        public IWebElement FindExistentElementByJQuery
        {
            get { return Find.Element(Seleno.PageObjects.Locators.By.jQuery("#RequiredBool")); }
        }

        public IWebElement FindOptionalNonExistentElement
        {
            get { return Find.OptionalElement(By.Id("RandomElement"), TimeSpan.FromSeconds(1)); }
        }

        public IWebElement FindOptionalNonExistentElementByJQuery
        {
            get { return Find.OptionalElement(Seleno.PageObjects.Locators.By.jQuery("#RandomElement"), TimeSpan.FromSeconds(1)); }
        }

        public IWebElement FindNonExistentElement(int timeoutInSeconds)
        {
            return Find.Element(By.Id("RandomElement"), TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public IWebElement FindNonExistentElementByJQuery(int timeoutInSeconds)
        {
            return Find.Element(Seleno.PageObjects.Locators.By.jQuery("#RandomElement"), TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public void AssertElementContainsValue(string id, string value)
        {
            AssertThatElements.ConformTo(By.Id(id), es => ThrowIfElementDoesNotContainValue(value, es));
        }

        private static void ThrowIfElementDoesNotContainValue(string value, IEnumerable<IWebElement> es)
        {
            if (es.Any(e => !e.GetControlValueAs<string>().Contains(value)))
                throw new SelenoException();
        }

        public void AssertElementContainsValueWithJquery(string id, string value)
        {
            AssertThatElements.ConformTo(
                Seleno.PageObjects.Locators.By.jQuery(string.Format("#{0}", id)), 
                es => ThrowIfElementDoesNotContainValue(value, es));
        }
    }
}