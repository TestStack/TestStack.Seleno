using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Controls;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageReader<TViewModel> : IPageReader<TViewModel> where TViewModel : class ,new()
    {
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IElementFinder _elementFinder;
        private readonly IComponentFactory _componentFactory;

        public PageReader(IScriptExecutor scriptExecutor, IElementFinder elementFinder, IComponentFactory componentFactory)
        {
            _scriptExecutor = scriptExecutor;
            _elementFinder = elementFinder;
            _componentFactory = componentFactory;
        }

        public TViewModel ModelFromPage()
        {
            var type = typeof(TViewModel);

            var instance = new TViewModel();
            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name;
                var javascriptExtractor = string.Format("$('#{0}').val()", propertyName);
                var typedValue = _scriptExecutor.ScriptAndReturn(javascriptExtractor, property.PropertyType);

                if (property.CanWriteToProperty(typedValue))
                {
                    property.SetValue(instance, typedValue, null);
                }
            }
            return instance;
        }

        public virtual bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<CheckBox>(checkBoxPropertySelector, maxWaitInSeconds)
                .Checked;
        }

        public string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<TextBox>(textBoxPropertySelector)
                .Value;
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field, int maxWaitInSeconds = 5)
        {
            var name = ExpressionHelper.GetExpressionText(field);
            var id = TagBuilder.CreateSanitizedId(name);
            return _elementFinder.Element(By.Id(id), maxWaitInSeconds);
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return _scriptExecutor.ScriptAndReturn<bool>(javascriptExpression);
        }

        public TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, int maxWaitInSeconds = 5)
        {
            return ElementFor(propertySelector, maxWaitInSeconds).GetAttributeAs<TProperty>(attributeName);
        }

        public TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<TextBox>(propertySelector, maxWaitInSeconds)
                .ValueAs<TProperty>();
        }
       
        public TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int maxWaitInSeconds = 5)
        {
            return ElementFor(propertySelector, maxWaitInSeconds).Text.TryConvertTo(default(TProperty));
        }

        public TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<DropDown>(dropDownSelector, maxWaitInSeconds)
                .SelectedElementAs<TField>();
        }
        
        public string SelectedOptionTextInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<DropDown>(dropDownSelector, maxWaitInSeconds)
                .SelectedElementText;
        }

        public bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<RadioButtonGroup>(radioGroupButtonSelector, maxWaitInSeconds)
                .HasSelectedElement;
        }

        public TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<RadioButtonGroup>(radioGroupButtonSelector, maxWaitInSeconds)
                .SelectedElementAs<TProperty>();
        }


        public string TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, int maxWaitInSeconds = 5)
        {
            return _componentFactory
                .HtmlControlFor<TextArea>(textAreaPropertySelector, maxWaitInSeconds)
                .Content;
        }
    }
}