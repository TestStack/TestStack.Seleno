using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        IWebElement ElementFor<TProperty>(Expression<Func<TViewModel, TProperty>> field, int waitInSeconds);
        bool ExistsAndIsVisible<TProperty>(Expression<Func<TViewModel, TProperty>> field);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName);
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 0);
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector);
        
        bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector);
        string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector);
        string SelectedOptionTextInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector,int waitInSeconds = 0);
        TProperty SelectedOptionValueInDropDown<TProperty>(Expression<Func<TViewModel, TProperty>> dropDownSelector, int waitInSeconds = 0);
        TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 0);
        string[] TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, int waitInSeconds = 0);


    }
}