using System.Linq.Expressions;

namespace TestStack.Seleno.PageObjects.Controls
{
    internal static class HtmlControlExtensions
    {
        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, LambdaExpression propertySelector, int maxWaitInSeconds = 20)
            where THtmlControl : IHtmlControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.ViewModelPropertySelector = propertySelector;
                result.WaitInSecondsUntilElementAvailable = maxWaitInSeconds;

                return (THtmlControl)(IHtmlControl)result;
            }

            return htmlControl;
        }

        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, string id, int maxWaitInSeconds = 5)
            where THtmlControl : IHtmlControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.Id = id;
                result.WaitInSecondsUntilElementAvailable = maxWaitInSeconds;

                return (THtmlControl)(IHtmlControl)result;
            }

            return htmlControl;
        }


    }
}