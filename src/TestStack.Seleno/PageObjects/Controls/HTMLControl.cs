using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
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

        protected PropertyInfo ViewModelProperty
        {
            get
            {
                if (ViewModelPropertySelector == null)
                {
                    throw new ArgumentException("A view model selector expression to the property for the control must be specified");
                }

                return ViewModelPropertySelector.GetPropertyFromLambda();
            }
        }

        internal int WaitInSecondsUntilElementAvailable { get; set; }

        internal LambdaExpression ViewModelPropertySelector { get; set; }

        private string _id;
        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id))
                {
                    _id = TagBuilder.CreateSanitizedId(Name);
                }
                return _id;
            }

            internal set { _id = value; }
        }

        public string Name
        {
            get { return ViewModelProperty != null ? ViewModelProperty.Name : Id; }
        }

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
            return Execute().ScriptAndReturn<TReturn>(string.Format("$('#{0}').attr('{1}')", Id, attributeName));
        }

        public void SetAttributeValue<TValue>(string attributeName, TValue value)
        {
            Execute().ExecuteScript(string.Format("$('#{0}').attr('{1}','{2}')", Id, attributeName, value));
        }

        public void RemoveAttribute(string attributeName)
        {
            Execute().ExecuteScript(string.Format("$('#{0}').removeAttr('{1}')", Id, attributeName));
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