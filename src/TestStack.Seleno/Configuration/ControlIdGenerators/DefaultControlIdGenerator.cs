using System.Linq.Expressions;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration.ControlIdGenerators
{
    /// <summary>
    /// Default control id generator - uses the property name of the last property in the lambda expression chain.
    /// </summary>
    public class DefaultControlIdGenerator : IControlIdGenerator
    {
        public string GetControlId(LambdaExpression expression)
        {
            return expression.GetPropertyFromLambda().Name;
        }
    }
}
