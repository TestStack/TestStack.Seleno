using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration.Screenshots
{
    /// <summary>
    /// Camera that saves screenshots to a file.
    /// </summary>
    public class FileCamera : ICamera
    {
        private readonly string _screenShotPath;

        /// <summary>
        /// Constructs a FileCamera.
        /// </summary>
        /// <param name="screenShotPath">The file system directory to save screenshots in</param>
        public FileCamera(string screenShotPath)
        {
            _screenShotPath = screenShotPath;
        }

        public void TakeScreenshot(string fileName = null)
        {
            var screenshot = ScreenshotTaker.GetScreenshot();

            if (!Directory.Exists(_screenShotPath))
                Directory.CreateDirectory(_screenShotPath);

            var windowTitle = Browser.Title;
            fileName = fileName ?? string.Format("{0}{1}.png", windowTitle, SystemTime.Now().ToFileTime()).Replace(':', '.');
            var outputPath = Path.Combine(_screenShotPath, fileName);

            var pathChars = Path.GetInvalidPathChars();

            var stringBuilder = new StringBuilder(outputPath);

            foreach (var item in pathChars)
                stringBuilder.Replace(item, '.');

            var screenShotPath = stringBuilder.ToString();
            screenshot.SaveAsFile(screenShotPath, ImageFormat.Png);
        }

        public ITakesScreenshot ScreenshotTaker { get; set; }
        public IWebDriver Browser { get; set; }
    }
}