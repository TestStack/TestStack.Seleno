using System;
using System.Linq.Expressions;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IHtmlControl
    {
        string Id { get; }
        string Name { get; }
        string Title { get; set; }
        bool ReadOnly { get; set; }
        bool Disabled { get; set; }

        TReturn AttributeValueAs<TReturn>(string attributeName);
        void SetAttributeValue<TValue>(string attributeName, TValue value);
    }
    
    public abstract class HTMLControl : UiComponent, IHtmlControl
    {
        public const string TitleAttribute = "title";
        public const string ReadOnlyAttribute = "readonly";
        public const string DisabledAttribute = "disabled";

        internal TimeSpan WaitUntilElementAvailable { get; set; }

        internal LambdaExpression ViewModelPropertySelector { get; set; }

        private string _id;
        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id))
                {
                    _id = ControlIdGenerator.GetControlId(Name);
                }
                return _id;
            }

            internal set { _id = value; }
        }

        public string Name => ViewModelPropertySelector != null ? ControlIdGenerator.GetControlName(ViewModelPropertySelector) : Id;

        public string Title
        {
            get { return AttributeValueAs<string>(TitleAttribute); }
            set { SetAttributeValue(TitleAttribute, value); }
        }

        public bool ReadOnly
        {
            get { return AttributeValueAs<object>(ReadOnlyAttribute) != null; }
            set { AddOrRemoveAttribute(TitleAttribute, value); }
        }

        public bool Disabled
        {
            get { return AttributeValueAs<object>(DisabledAttribute) != null; }
            set { AddOrRemoveAttribute(DisabledAttribute, value); }
        }

        public TReturn AttributeValueAs<TReturn>(string attributeName)
        {
            return Execute.ScriptAndReturn<TReturn>($"$('#{Id}').attr('{attributeName}')");
        }

        public void SetAttributeValue<TValue>(string attributeName, TValue value)
        {
            Execute.Script($@"$('#{Id}').attr('{attributeName}', ""{value.ToString().ToJavaScriptString()}"")");
        }

        public void RemoveAttribute(string attributeName)
        {
            Execute.Script($"$('#{Id}').removeAttr('{attributeName}')");
        }

        protected void AddOrRemoveAttribute(string attributeName, bool addOrRemove)
        {
            if (addOrRemove)
            {
                SetAttributeValue(attributeName, string.Empty);
            }
            else
            {
                RemoveAttribute(attributeName);
            }
        }
    }
}