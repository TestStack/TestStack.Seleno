using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Screenshots
{
    public class FileCamera : ICamera
    {
        private readonly string _screenShotPath;

        public FileCamera(string screenShotPath)
        {
            _screenShotPath = screenShotPath;
        }

        public void TakeScreenshot(string fileName = null)
        {
            var browser = SelenoApplicationRunner.Host.Browser;
            var camera = (ITakesScreenshot)browser;
            var screenshot = camera.GetScreenshot();

            if (!Directory.Exists(_screenShotPath))
                Directory.CreateDirectory(_screenShotPath);

            var windowTitle = browser.Title;
            fileName = fileName ?? string.Format("{0}{1}.png", windowTitle, DateTime.Now.ToFileTime()).Replace(':', '.');
            var outputPath = Path.Combine(_screenShotPath, fileName);

            var pathChars = Path.GetInvalidPathChars();

            var stringBuilder = new StringBuilder(outputPath);

            foreach (var item in pathChars)
                stringBuilder.Replace(item, '.');

            var screenShotPath = stringBuilder.ToString();
            screenshot.SaveAsFile(screenShotPath, ImageFormat.Png);
        }
    }
}