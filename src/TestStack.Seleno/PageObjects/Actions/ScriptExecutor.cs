using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Actions
{
    public class ScriptExecutor : IScriptExecutor
    {
        protected RemoteWebDriver Browser;
        private readonly IElementFinder _finder;

        internal ScriptExecutor(RemoteWebDriver browser, IElementFinder finder)
        {
            Browser = browser;
            _finder = finder;
        }

        public IWebElement ActionOnLocator(By findElement, Action<IWebElement> action)
        {
            try
            {
                var element = Browser.FindElement(findElement);
                action(element);
                return element;
            }
            catch (Exception)
            {
                TakeScreenshot();
                throw;
            }
        }

        public TResult ActionOnLocator<TResult>(By findElement, Func<IWebElement, TResult> func)
        {
            try
            {
                var element = Browser.FindElement(findElement);
                return func(element);
            }
            catch (Exception)
            {
                TakeScreenshot();
                throw;
            }
        }

        public IWebElement WithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
        {
            try
            {
                var element = _finder.ElementWithWait(findElement, waitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                TakeScreenshot();
                throw;
            }
        }

        public object ScriptAndReturn(string javascriptToBeExecuted, Type returnType, IJavaScriptExecutor javaScriptExecutor = null)
        {
            javaScriptExecutor = javaScriptExecutor ?? Browser;

            object untypedValue = javaScriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
            object result = untypedValue.TryConvertTo(returnType, null);

            return result;
        }

        private void TakeScreenshot(string fileName = null)
        {
            var pathFromConfig = Configurator.ScreenShotPath;
            var camera = (ITakesScreenshot)Browser;
            var screenshot = camera.GetScreenshot();

            if (!Directory.Exists(pathFromConfig))
                Directory.CreateDirectory(pathFromConfig);

            var windowTitle = Browser.Title;
            fileName = fileName ?? string.Format("{0}{1}.png", windowTitle, DateTime.Now.ToFileTime()).Replace(':', '.');
            var outputPath = Path.Combine(pathFromConfig, fileName);

            var pathChars = Path.GetInvalidPathChars();

            var stringBuilder = new StringBuilder(outputPath);

            foreach (var item in pathChars)
                stringBuilder.Replace(item, '.');

            var screenShotPath = stringBuilder.ToString();
            screenshot.SaveAsFile(screenShotPath, ImageFormat.Png);
        }

    }
}