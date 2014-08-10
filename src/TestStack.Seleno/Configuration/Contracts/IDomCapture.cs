using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration.Contracts
{
    public interface IDomCapture
    {
        /// <summary>
        /// The browser that is viewing the page the DOM was captured from.
        /// </summary>
        IWebDriver Browser { get; set; }

        /// <summary>
        /// Captures the DOM using the given filename (if specified).
        /// </summary>
        /// <param name="fileName">The filename to use to save the DOM as</param>
        void CaptureDom(string fileName = null);
    }
}
