using System;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field, int waitInSeconds);
        bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName);
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 0);
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector);
        
        bool CheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field);
        string TextboxValue<TField>(Expression<Func<TViewModel, TField>> field);
        string SelectedOptionTextInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector,int waitInSeconds = 0);
        TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 0);
        TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 0);
        
        
    }
}