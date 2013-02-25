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
    }
}