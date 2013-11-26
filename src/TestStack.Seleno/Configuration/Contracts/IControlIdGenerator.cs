using System.Linq.Expressions;

namespace TestStack.Seleno.Configuration.Contracts
{
    /// <summary>
    /// Generates ids for HTML controls.
    /// </summary>
    public interface IControlIdGenerator
    {
        /// <summary>
        /// Generates a control id from a lambda expression of a property representing the control.
        /// </summary>
        /// <param name="expression">A lambda expression of the property representing the control</param>
        /// <returns>The id of the HTML control</returns>
        string GetControlId(LambdaExpression expression);
    }
}
