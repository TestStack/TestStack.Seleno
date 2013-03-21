using System;
using System.Linq.Expressions;

namespace TestStack.Seleno.PageObjects.Controls
{
    internal static class HtmlControlExtensions
    {
        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, LambdaExpression propertySelector, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.ViewModelPropertySelector = propertySelector;
                result.WaitUntilElementAvailable = maxWait;

                return (THtmlControl)(IHtmlControl)result;
            }

            return htmlControl;
        }

        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, string id, TimeSpan maxWait = default(TimeSpan))
            where THtmlControl : IHtmlControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.Id = id;
                result.WaitUntilElementAvailable = maxWait;

                return (THtmlControl)(IHtmlControl)result;
            }

            return htmlControl;
        }
    }
}