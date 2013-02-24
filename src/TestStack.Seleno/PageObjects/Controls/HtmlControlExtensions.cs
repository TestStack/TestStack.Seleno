using System.Linq.Expressions;

namespace TestStack.Seleno.PageObjects.Controls
{
    internal static class HtmlControlExtensions
    {
        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, LambdaExpression propertySelector, int waitInSecondsUntilElementAvailable = 20)
            where THtmlControl : IHTMLControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.ViewModelPropertySelector = propertySelector;
                result.WaitInSecondsUntilElementAvailable = waitInSecondsUntilElementAvailable;

                return (THtmlControl)(IHTMLControl)result;
            }

            return htmlControl;
        }

        internal static THtmlControl Initialize<THtmlControl>(this THtmlControl htmlControl, string id, int waitInSecondsUntilElementAvailable = 20)
            where THtmlControl : IHTMLControl
        {
            var result = htmlControl as HTMLControl;

            if (result != null)
            {
                result.Id = id;
                result.WaitInSecondsUntilElementAvailable = waitInSecondsUntilElementAvailable;

                return (THtmlControl)(IHTMLControl)result;
            }

            return htmlControl;
        }


    }
}