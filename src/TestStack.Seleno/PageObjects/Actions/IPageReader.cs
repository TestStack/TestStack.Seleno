using System;
using System.Linq.Expressions;
using OpenQA.Selenium;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        IWebElement ElementFor<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, TimeSpan maxWait = default(TimeSpan));
        bool ExistsAndIsVisible<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector);
        bool ExistsAndIsVisible(By.jQueryBy jqueryBy);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, TimeSpan maxWait = default(TimeSpan));
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, TimeSpan maxWait = default(TimeSpan));
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, TimeSpan maxWait = default(TimeSpan));

        bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, TimeSpan maxWait = default(TimeSpan));
        string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, TimeSpan maxWait = default(TimeSpan));
        string SelectedOptionTextInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector, TimeSpan maxWait = default(TimeSpan));
        TProperty SelectedOptionValueInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector, TimeSpan maxWait = default(TimeSpan));
        TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, TimeSpan maxWait = default(TimeSpan));
        bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, TimeSpan maxWait = default(TimeSpan));
        string TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, TimeSpan maxWait = default(TimeSpan));
    }
}