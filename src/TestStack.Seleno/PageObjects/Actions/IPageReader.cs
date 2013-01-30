using System;
using System.Linq.Expressions;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageReader<TViewModel> where TViewModel : class, new()
    {
        TViewModel ModelFromPage();
        bool CheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field);
        string TextboxValue<TField>(Expression<Func<TViewModel, TField>> field);
        IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field);
        bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field);
        TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName);
        TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector);
        TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector);
    }
}