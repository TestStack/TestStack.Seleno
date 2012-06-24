using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.Screenshots
{
    public class FileCamera : ICamera
    {
        public RemoteWebDriver Browser { get; set; }
        private readonly string _screenShotPath;
        private readonly ImageFormat _imageFormat;

        private FileCamera() { }

        public FileCamera(string screenShotPath, ImageFormat imageFormat)
        {
            _screenShotPath = screenShotPath;
            _imageFormat = imageFormat;
        }

        public void TakeScreenshot(string fileName = null)
        {
            var camera = (ITakesScreenshot)Browser;
            var screenshot = camera.GetScreenshot();

            if (!Directory.Exists(_screenShotPath))
                Directory.CreateDirectory(_screenShotPath);

            var windowTitle = Browser.Title;
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