using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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

        protected virtual PropertyInfo ViewModelProperty
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

        internal TimeSpan WaitUntilElementAvailable { get; set; }

        internal LambdaExpression ViewModelPropertySelector { get; set; }

        private string _id;
        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id))
                {
                    _id = Html401IdUtil.CreateSanitizedId(Name);
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
            return Execute.ScriptAndReturn<TReturn>(string.Format("$('#{0}').attr('{1}')", Id, attributeName));
        }

        public void SetAttributeValue<TValue>(string attributeName, TValue value)
        {
            Execute.ExecuteScript(string.Format(@"$('#{0}').attr('{1}', ""{2}"")", Id, attributeName, value.ToString().ToJavaScriptString()));
        }

        public void RemoveAttribute(string attributeName)
        {
            Execute.ExecuteScript(string.Format("$('#{0}').removeAttr('{1}')", Id, attributeName));
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


        /// <summary>
        /// Copied from System.Web.Mvc.TagBuilder
        /// </summary>
        private static class Html401IdUtil
        {
            public static string CreateSanitizedId(string originalId)
            {
                if (string.IsNullOrEmpty(originalId))
                    return null;
                char c1 = originalId[0];
                if (!IsLetter(c1))
                    return null;
                var stringBuilder = new StringBuilder(originalId.Length);
                stringBuilder.Append(c1);
                for (var index = 1; index < originalId.Length; ++index)
                {
                    char c2 = originalId[index];
                    if (IsValidIdCharacter(c2))
                        stringBuilder.Append(c2);
                    else
                        stringBuilder.Append("_");
                }
                return stringBuilder.ToString();
            }

            private static bool IsAllowableSpecialCharacter(char c)
            {
                switch (c)
                {
                    case '-':
                    case ':':
                    case '_':
                        return true;
                    default:
                        return false;
                }
            }

            private static bool IsDigit(char c)
            {
                if (48 <= c)
                    return c <= 57;
                return false;
            }

            private static bool IsLetter(char c)
            {
                if (65 <= c && c <= 90)
                    return true;
                if (97 <= c)
                    return c <= 122;
                return false;
            }

            private static bool IsValidIdCharacter(char c)
            {
                if (!IsLetter(c) && !IsDigit(c))
                    return IsAllowableSpecialCharacter(c);
                return true;
            }
        }
    }
}