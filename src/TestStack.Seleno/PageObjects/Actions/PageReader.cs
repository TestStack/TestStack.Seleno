using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageReader<TViewModel> : IPageReader<TViewModel> where TViewModel : class, new()
    {
        private readonly IExecutor _executor;
        private readonly IElementFinder _elementFinder;
        private readonly IComponentFactory _componentFactory;

        public PageReader(IExecutor executor, IElementFinder elementFinder, IComponentFactory componentFactory)
        {
            _executor = executor;
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
                var typedValue = _executor.ScriptAndReturn(javascriptExtractor, property.PropertyType);

                if (property.CanWriteToProperty(typedValue))
                {
                    property.SetValue(instance, typedValue, null);
                }
            }
            return instance;
        }

        public virtual bool CheckBoxValue<TProperty>(Expression<Func<TViewModel, TProperty>> checkBoxPropertySelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<CheckBox>(checkBoxPropertySelector, maxWait)
                .Checked;
        }

        public string TextboxValue<TProperty>(Expression<Func<TViewModel, TProperty>> textBoxPropertySelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<TextBox>(textBoxPropertySelector, maxWait)
                .Value;
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field, TimeSpan maxWait = default(TimeSpan))
        {
            var name = ExpressionHelper.GetExpressionText(field);
            var id = TagBuilder.CreateSanitizedId(name);
            return _elementFinder.Element(By.Id(id), maxWait);
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return _executor.ScriptAndReturn<bool>(javascriptExpression);
        }

        public TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName, TimeSpan maxWait = default(TimeSpan))
        {
            return ElementFor(propertySelector, maxWait).GetAttributeAs<TProperty>(attributeName);
        }

        public TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<TextBox>(propertySelector, maxWait)
                .ValueAs<TProperty>();
        }

        public TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, TimeSpan maxWait = default(TimeSpan))
        {
            return ElementFor(propertySelector, maxWait).Text.TryConvertTo(default(TProperty));
        }

        public TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<DropDown>(dropDownSelector, maxWait)
                .SelectedElementAs<TField>();
        }

        public string SelectedOptionTextInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<DropDown>(dropDownSelector, maxWait)
                .SelectedElementText;
        }

        public bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<RadioButtonGroup>(radioGroupButtonSelector, maxWait)
                .HasSelectedElement;
        }

        public TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<RadioButtonGroup>(radioGroupButtonSelector, maxWait)
                .SelectedElementAs<TProperty>();
        }

        public string TextAreaContent(Expression<Func<TViewModel, string>> textAreaPropertySelector, TimeSpan maxWait = default(TimeSpan))
        {
            return _componentFactory
                .HtmlControlFor<TextArea>(textAreaPropertySelector, maxWait)
                .Content;
        }
    }
}