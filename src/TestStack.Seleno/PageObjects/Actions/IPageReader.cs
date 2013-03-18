using System;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        IWebElement ElementFor<TProperty>(Expression<Func<TViewModel, TProperty>> field, int maxWaitInSeconds = 5);
        bool ExistsAndIsVisible<TProperty>(Expression<Func<TViewModel, TProperty>> field);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, int maxWaitInSeconds = 5);
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int maxWaitInSeconds = 5);
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int maxWaitInSeconds = 5);

        bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, int maxWaitInSeconds = 5);
        string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, int maxWaitInSeconds = 5);
        string SelectedOptionTextInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector,int maxWaitInSeconds = 5);
        TProperty SelectedOptionValueInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector, int maxWaitInSeconds = 5);
        TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int maxWaitInSeconds = 5);
        bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int maxWaitInSeconds = 5);
        string TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, int maxWaitInSeconds = 5);
    }
}