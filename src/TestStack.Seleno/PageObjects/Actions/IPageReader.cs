using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        IWebElement ElementFor<TProperty>(Expression<Func<TViewModel, TProperty>> field, int waitInSeconds = 20);
        bool ExistsAndIsVisible<TProperty>(Expression<Func<TViewModel, TProperty>> field);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, int waitInSeconds = 20);
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 20);
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 20);

        bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, int waitInSeconds = 20);
        string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, int waitInSeconds = 20);
        string SelectedOptionTextInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector,int waitInSeconds = 20);
        TProperty SelectedOptionValueInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector, int waitInSeconds = 20);
        TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 20);
        bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 20);
        string[] TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, int waitInSeconds = 20);


    }
}