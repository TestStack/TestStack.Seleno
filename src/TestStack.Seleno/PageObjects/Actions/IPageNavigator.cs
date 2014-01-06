using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    /// <summary>
    /// Navigate to a different page.
    /// </summary>
    public interface IPageNavigator
    {
        /// <summary>
        /// Navigate to a different page by clicking on an element in the page.
        /// </summary>
        /// <typeparam name="TPage">The page object type for the expected resultant page</typeparam>
        /// <param name="elementToClick">The element to find in the current page and click</param>
        /// <param name="maxWait">Maximum amount of time to wait for the element to become available to click</param>
        /// <returns>An instantiated and initialised <see cref="TPage"/> page object</returns>
        TPage To<TPage>(By elementToClick, TimeSpan maxWait = default(TimeSpan))
            where TPage : UiComponent, new();

        /// <summary>
        /// Navigate to a different page by clicking on an element in the page.
        /// </summary>
        /// <typeparam name="TPage">The page object type for the expected resultant page</typeparam>
        /// <param name="jQueryElementToClick">jQuery expression to find an element</param>
        /// <param name="maxWait">Maximum amount of time to wait for the element to become available to click</param>
        /// <returns>An instantiated and initialised <see cref="TPage"/> page object</returns>
        TPage To<TPage>(Locators.By.jQueryBy jQueryElementToClick, TimeSpan maxWait = default(TimeSpan))
            where TPage : UiComponent, new();

        /// <summary>
        /// Navigate to a different page by going to a URL (relative to the root URL of the application under test).
        /// </summary>
        /// <typeparam name="TPage">The page object type for the expected resultant page</typeparam>
        /// <param name="url">The URL, either relative to the root URL of the application under test or the absolute URL, to navigate to</param>
        /// <returns>An instantiated and initialised <see cref="TPage"/> page object</returns>
        TPage To<TPage>(string url)
            where TPage : UiComponent, new();

        /// <summary>
        /// Navigate to a different page by going to a URL by an ASP.NET MVC route.
        /// </summary>
        /// <typeparam name="TController">The type of the controller that is serving the page you are navigating to</typeparam>
        /// <typeparam name="TPage">The page object type for the expected resultant page</typeparam>
        /// <param name="action">An expression representing the controller action being navigated to</param>
        /// <param name="routeValues">Extra route data to use to generate the route</param>
        /// <returns>An instantiated and initialised <see cref="TPage"/> page object</returns>
        TPage To<TController, TPage>(Expression<Action<TController>> action, IDictionary<string, object> routeValues = null)
            where TController : Controller
            where TPage : UiComponent, new();

        /// <summary>
        /// Navigate to a different page by going to a URL by an ASP.NET MVC route.
        /// </summary>
        /// <typeparam name="TController">The type of the controller that is serving the page you are navigating to</typeparam>
        /// <typeparam name="TPage">The page object type for the expected resultant page</typeparam>
        /// <param name="action">An expression representing the controller action being navigated to</param>
        /// <param name="routeValues">Extra route data to use to generate the route as an anonymous object</param>
        /// <returns>An instantiated and initialised <see cref="TPage"/> page object</returns>
        TPage To<TController, TPage>(Expression<Action<TController>> action, object routeValues)
            where TController : Controller
            where TPage : UiComponent, new();

        /// <summary>Obsolete</summary>
        [Obsolete("Obsolete: Use To<TPage> or HttpClient instead. See BREAKING_CHANGES.md on the Github repository under version 0.4", true)]
        void To(By elementToClick);
    }
}