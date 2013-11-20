using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Screenshots
{
    /// <summary>
    /// Camera that doesn't take any screenshots.
    /// </summary>
    public class NullCamera : ICamera
    {
        public void TakeScreenshot(string fileName = null)
        {
            // do nothing
        }

        public ITakesScreenshot ScreenshotTaker { get; set; }
        public IWebDriver Browser { get; set; }
    }
}