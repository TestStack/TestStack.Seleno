using System.Linq.Expressions;
using System.Web.Mvc;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.ControlIdGenerators
{
    /// <summary>
    /// Default control id generator - uses the same algorithm ASP.NET MVC uses to generate control ids.
    /// </summary>
    public class MvcControlIdGenerator : IControlIdGenerator
    {
        public string GetControlId(LambdaExpression expression)
        {
            return ExpressionHelper.GetExpressionText(expression);
        }
    }
}
