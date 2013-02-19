using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Components;
using By = TestStack.Seleno.PageObjects.Locators.By;


namespace TestStack.Seleno.PageObjects.Actions
{
    internal class PageReader<TViewModel> : IPageReader<TViewModel> where TViewModel : class ,new()
    {
        private readonly IWebDriver _browser;
        private readonly IScriptExecutor _scriptExecutor;
        private readonly IElementFinder _elementFinder;
        private readonly IComponentFactory _componentFactory;

        public PageReader(IWebDriver browser, IScriptExecutor scriptExecutor, IElementFinder elementFinder, IComponentFactory componentFactory)
        {
            _browser = browser;
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

        public bool CheckBoxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(OpenQA.Selenium.By.Name(name)).Selected;
        }

        public string TextboxValue<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            return _browser.FindElement(OpenQA.Selenium.By.Name(name)).GetAttribute("value");
        }

        public IWebElement ElementFor<TField>(Expression<Func<TViewModel, TField>> field, int waitInSeconds = 0)
        {
            var name = ExpressionHelper.GetExpressionText(field);
            var id = TagBuilder.CreateSanitizedId(name);
            return _elementFinder.TryFindElement(By.Id(id), waitInSeconds);
        }

        public bool ExistsAndIsVisible<TField>(Expression<Func<TViewModel, TField>> field)
        {
            var name = ExpressionHelper.GetExpressionText(field);

            var javascriptExpression = string.Format("$('#{0}').is(':visible')", name);
            return _browser.ExecuteScriptAndReturn<bool>(javascriptExpression);
        }

        public TProperty GetAttributeAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector, string attributeName)
        {
            return ElementFor(propertySelector).GetAttributeAs<TProperty>(attributeName);
        }

        public TProperty GetValueFromTextBox<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector)
        {
            return GetAttributeAsType(propertySelector, "value");
        }

        public TProperty TextAsType<TProperty>(Expression<Func<TViewModel, TProperty>> propertySelector)
        {
            return ElementFor(propertySelector).Text.TryConvertTo(default(TProperty));
        }

        public TField SelectedOptionValueInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 0)
        {
            //var dropDownElement = FindSelectionOptionForDropDown(dropDownSelector, waitInSeconds);
            return
                _componentFactory
                    .HtmlControlFor<IDropDown>(dropDownSelector, waitInSeconds)
                    .SelectedElementAs<TField>();

            /*return dropDownElement.GetControlValueAs<TField>();*/
        }
        
        public string SelectedOptionTextInDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds = 0)
        {
            return
                _componentFactory
                    .HtmlControlFor<IDropDown>(dropDownSelector, waitInSeconds)
                    .SelectedElementText;

            /*var dropDownElement = FindSelectionOptionForDropDown(dropDownSelector, waitInSeconds);
            return dropDownElement.Text;*/
        }

        public bool HasSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 0)
        {
            
            return
                _componentFactory
                .HtmlControlFor<ISelectableHtmlControl>(radioGroupButtonSelector,waitInSeconds)
                .HasSelectedElement;
            //return FindSelectedRadioButtonInRadioGroup(radioGroupButtonSelector, waitInSeconds) != null;
        }

        public TProperty SelectedButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds = 0)
        {
            return
                _componentFactory
                    .HtmlControlFor<ISelectableHtmlControl>(radioGroupButtonSelector, waitInSeconds)
                    .SelectedElementAs<TProperty>();

            //var selectedRadioButtonElement = FindSelectedRadioButtonInRadioGroup(radioGroupButtonSelector, waitInSeconds);
            //if (selectedRadioButtonElement == null)
            //{
            //    throw new NoSuchElementException("No selected radio button has been found");
            //}
            //return selectedRadioButtonElement.GetControlValueAs<TProperty>();
        }

        //private IWebElement FindSelectedRadioButtonInRadioGroup<TProperty>(Expression<Func<TViewModel, TProperty>> radioGroupButtonSelector, int waitInSeconds)
        //{
        //    var selector = string.Format("$('input[type=radio][name={0}]:checked')",
        //                               ExpressionHelper.GetExpressionText(radioGroupButtonSelector));

        //    return _elementFinder.TryFindElement(By.jQuery(selector), waitInSeconds);
        //}

        //private IWebElement FindSelectionOptionForDropDown<TField>(Expression<Func<TViewModel, TField>> dropDownSelector, int waitInSeconds)
        //{
        //    var dropDownId = TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(dropDownSelector));
        //    var selector = string.Format("$('#{0} option:selected')", dropDownId);

        //    return _elementFinder.ElementWithWait(By.jQuery(selector), waitInSeconds);
        //}

    }
}