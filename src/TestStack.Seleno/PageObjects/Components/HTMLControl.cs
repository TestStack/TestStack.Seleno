using System.Linq.Expressions;
using System.Web.Mvc;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Components
{
    public class HTMLControl : UiComponent, IHTMLControl
    {
        private string _id;

        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id) && PropertySelector != null)
                {
                    var property = PropertySelector.GetPropertyFromLambda();
                    _id = TagBuilder.CreateSanitizedId(property.Name);
                }
                return _id;
            }

            internal set { _id = value; }
        }

        internal int WaitInSecondsUntilElementAvailable { get; set; }

        internal LambdaExpression PropertySelector { get; set; }   

    }
}