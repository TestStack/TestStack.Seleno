using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public abstract class HTMLControl : UiComponent, IHTMLControl
    {
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
            get { return ViewModelProperty.Name; }
        }

        public TReturn AttributeValueAs<TReturn>(string attributeName)
        {
            return Execute().ScriptAndReturn<TReturn>(string.Format("$('#{0}').attr({1})",Id,attributeName));
        }

    }
}