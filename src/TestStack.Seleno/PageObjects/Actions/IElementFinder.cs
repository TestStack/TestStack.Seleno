using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    /// <summary>
    /// Provides an API to find elements on the page.
    /// </summary>
    public interface IElementFinder
    {
        /// <summary>
        /// Locate an element using a Selenium Web Driver expression and return the <see cref="IWebElement"/> corresponding to that element.
        /// Throws an exception if the element isn't found.
        /// </summary>
        /// <example>
        /// Find().Element(By.Id("helloworld"))
        /// </example>
        /// <param name="findExpression">Selenium Web Driver expression to find an element</param>
        /// <param name="waitInSeconds">Maximum number of seconds to wait for the element to become available</param>
        /// <exception cref="NoSuchElementException">When the element isn't found</exception>
        /// <returns>The <see cref="IWebElement"/> representing the element</returns>
        IWebElement Element(By findExpression, int waitInSeconds = 20);

        /// <summary>
        /// Locate an element using a jQuery expression and return the <see cref="IWebElement"/> corresponding to that element.
        /// Throws an exception if the element isn't found.
        /// </summary>
        /// <example>
        /// Find().Element(By.jQuery("#helloworld"))
        /// </example>
        /// <param name="jQueryFindExpression">jQuery expression to find an element</param>
        /// <param name="waitInSeconds">Maximum number of seconds to wait for the element to become available</param>
        /// <exception cref="NoSuchElementException">When the element isn't found</exception>
        /// <returns>The <see cref="IWebElement"/> representing the element</returns>
        IWebElement Element(Locators.By.jQueryBy jQueryFindExpression, int waitInSeconds = 20);

        /// <summary>
        /// Locate an element that may or not be present using a Selenium Web Driver expression and return the <see cref="IWebElement"/> corresponding to that element.
        /// Returns null if the element isn't found.
        /// </summary>
        /// <example>
        /// Find().OptionalElement(By.Id("helloworld"))
        /// </example>
        /// <param name="findExpression">Selenium Web Driver expression to find an element</param>
        /// <returns>The <see cref="IWebElement"/> representing the element</returns>
        IWebElement OptionalElement(By findExpression);

        /// <summary>
        /// Locate an element that may or not be present using a jQuery expression and return the <see cref="IWebElement"/> corresponding to that element.
        /// Returns null if the element isn't found.
        /// </summary>
        /// <example>
        /// Find().OptionalElement(By.jQuery("#helloworld"))
        /// </example>
        /// <param name="jQueryFindExpression">jQuery expression to find an element</param>
        /// <returns>The <see cref="IWebElement"/> representing the element</returns>
        IWebElement OptionalElement(Locators.By.jQueryBy jQueryFindExpression);

        /// <summary>
        /// Obsolete: Use OptionalElement instead.
        /// </summary>
        [Obsolete("Use OptionalElement instead")]
        IWebElement TryFindElementByjQuery(Locators.By.jQueryBy by);
    }
}