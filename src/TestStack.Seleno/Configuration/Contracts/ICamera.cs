using System;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration.Contracts
{
    /// <summary>
    /// Class with ability to take screenshots.
    /// </summary>
    public interface ICamera
    {
        /// <summary>
        /// Take a screenshot using the given filename (if specified).
        /// </summary>
        /// <param name="fileName">The filename to use to save the screenshot</param>
        /// <param name="exception">Exception that raised the screenshot request</param>
        void TakeScreenshot(string fileName = null, Exception exception = null);

        /// <summary>
        /// The driver to take screenshots with - will be set after the camera is registered with Seleno.
        /// </summary>
        ITakesScreenshot ScreenshotTaker { get; set; }

        /// <summary>
        /// The browser that is viewing the page the screenshot is taken against.
        /// </summary>
        IWebDriver Browser { get; set; }
    }
}
