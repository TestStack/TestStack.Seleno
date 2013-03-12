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

        public bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, int waitInSeconds = 20)
        {
            return
                _componentFactory
                    .HtmlControlFor<ICheckBox>(checkBoxPropertySelector,waitInSeconds)
                    .Checked;
        }

        public string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, int waitInSeconds = 20)
        {
            return 
                _componentFactory
                    .HtmlControlFor<ITextBox>(textBoxPropertySelector)
                    .Value;
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field, int waitInSeconds = 20)
        {
            var name = ExpressionHelper.GetExpressionText(field);
            var id = TagBuilder.CreateSanitizedId(name);
            return _elementFinder.ElementWithWait(By.Id(id),waitInSeconds);
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return _scriptExecutor.ScriptAndReturn<bool>(javascriptExpression);
        }

        public TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, int waitInSeconds = 20)
        {
            return ElementFor(propertySelector,waitInSeconds).GetAttributeAs<TProperty>(attributeName);
        }

        public TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 20)
        {
            return
                _componentFactory
                    .HtmlControlFor<ITextBox>(propertySelector, waitInSeconds)
                    .ValueAs<TProperty>();
        }
       
        public TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, int waitInSeconds = 20)
        {
            return ElementFor(propertySelector, waitInSeconds).Text.TryConvertTo(default(TProperty));
        }

        public TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 20)
        {
            return
                _componentFactory
                    .HtmlControlFor<IDropDown>(dropDownSelector, waitInSeconds)
                    .SelectedElementAs<TField>();
        }
        
        public string SelectedOptionTextInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 20)
        {
            return
                _componentFactory
                    .HtmlControlFor<IDropDown>(dropDownSelector, waitInSeconds)
                    .SelectedElementText;
        }

        public bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 20)
        {
            return
                _componentFactory
                    .HtmlControlFor<IRadioButtonGroup>(radioGroupButtonSelector, waitInSeconds)
                    .HasSelectedElement;
        }

        public TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 0)
        {
            return
                _componentFactory
                    .HtmlControlFor<IRadioButtonGroup>(radioGroupButtonSelector, waitInSeconds)
                    .SelectedElementAs<TProperty>();
        }


        public string TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, int waitInSeconds = 0)
        {
            return
                _componentFactory
                    .HtmlControlFor<ITextArea>(textAreaPropertySelector, waitInSeconds)
                    .Content;
        }
    }
}