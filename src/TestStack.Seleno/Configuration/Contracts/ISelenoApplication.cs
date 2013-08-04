using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Castle.Core.Logging;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Configuration.Contracts
{
    /// <summary>
    /// Defines an instance of a running Seleno Application and associated web browser and Seleno test entry points.
    /// </summary>
    public interface ISelenoApplication : IDisposable
    {
        /// <summary>
        /// Initialise the application.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Access the Selenium Web Driver browsing the web application being tested.
        /// </summary>
        IWebDriver Browser { get; }

        /// <summary>
        /// Access the web server running the web application being tested.
        /// </summary>
        IWebServer WebServer { get; }

        /// <summary>
        /// Access the camera that takes screenshots of the web application being tested.
        /// </summary>
        ICamera Camera { get; }

        /// <summary>
        /// Access the logger.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Navigate to the initial page in a test by looking up the URL using an MVC controller and action and then return a page object instance of the specified type.
        /// Note: Requires you to have configured the seleno application with a Route Config.
        /// </summary>
        /// <typeparam name="TController">The type of the MVC controller containing the action to visit</typeparam>
        /// <typeparam name="TPage">The type of page object to intialise and return</typeparam>
        /// <param name="action">A lambda expression that specifies the action to navigate to</param>
        /// <returns>An initialised page object</returns>
        TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new();

        /// <summary>
        /// Navigate to the initial page in a test via a URL string relative to the root of the web application and then return a page object instance of the specified type.
        /// </summary>
        /// <typeparam name="TPage">The type of page object to initialise and return</typeparam>
        /// <param name="url">A URL string, either relative to the root of the web application being tested or absoluate URL</param>
        /// <returns>An initialised page object</returns>
        TPage NavigateToInitialPage<TPage>(string url = "")
            where TPage : UiComponent, new();
    }
}
