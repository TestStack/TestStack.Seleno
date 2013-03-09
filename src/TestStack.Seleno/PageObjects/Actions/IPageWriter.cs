using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageWriter<TModel> where TModel : class, new()
    {
        void Model(TModel viewModel, IDictionary<Type, Func<object, string>> propertyTypeHandling = null);
        [Obsolete("Use ReplaceInputValueWith instead")]
        void TextInField(string fieldName, string value);
        void ClearAndSendKeys<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, string value, bool clearFirst = true);
        void ClearAndSendKeys(string elementName, string value, bool clearFirst = true);
        void SetAttribute<TProperty>(Expression<Func<TModel, TProperty>> propertySelector,String attributeName, TProperty attributeValue);
        void ReplaceInputValueWith<TProperty>(Expression<Func<TModel, TProperty>> propertySelector, TProperty inputValue);
        void ReplaceInputValueWith(string inputName, string value);
        void TickCheckbox(Expression<Func<TModel, bool>> propertySelector, bool isTicked);
        void UpdateTextAreaContent(Expression<Func<TModel, string>> textAreaPropertySelector, string content,int waitInSeconds = 0);
        void UpdateTextAreaContent(Expression<Func<TModel, string>> textAreaPropertySelector, string[] multiLineContent, int waitInSeconds = 0);

        void SelectByOptionValueInDropDown<TProperty>(Expression<Func<TModel, TProperty>> dropDownSelector,TProperty optionValue);
        void SelectByOptionTextInDropDown<TProperty>(Expression<Func<TModel, TProperty>> dropDownSelector,string optionText);
        void SelectButtonInRadioGroup<TProperty>(Expression<Func<TModel, TProperty>> radioGroupButtonSelector, TProperty buttonValue);
    }
}