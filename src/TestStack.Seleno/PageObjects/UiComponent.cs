using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal readonly RemoteWebDriver Browser;
        private readonly PageNavigator _navigator;
        protected readonly ElementFinder ElementFinder;
        protected readonly ScriptExecutor ScriptExecutor;

        public UiComponent()
        {
            Browser = SelenoApplicationRunner.Host.Browser as RemoteWebDriver;
            ElementFinder = new ElementFinder(Browser);
            ScriptExecutor = new ScriptExecutor(Browser, ElementFinder);
            _navigator = new PageNavigator(Browser, ScriptExecutor);
        }

        protected IPageNavigator Navigate()
        {
            return _navigator;
        }

        protected IScriptExecutor Execute()
        {
            return ScriptExecutor;
        }

        protected IElementFinder Find()
        {
            return ElementFinder;
        }

        protected TableReader<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            return new TableReader<TModel>(gridId) { Browser = Browser };
        }

        public ElementAssert AssertThatElements(By selector)
        {
            return new ElementAssert(this, selector);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            return new TComponent();
        }

        public void WaitForAjaxCallsToFinish(int timeOutInSecond = 10)
        {
            var stillGoing = true;
            int waitedFor = 0;

            while (stillGoing)
            {
                Thread.Sleep(Configurator.WaitForAjaxPollingInterval);
                waitedFor++;

                stillGoing = !(bool)Browser.ExecuteScript("return jQuery.active == 0");
                if (waitedFor > timeOutInSecond)
                    throw new SelenoException(
                        string.Format("Wait for AJAX timed out after waiting for {0} seconds", timeOutInSecond));
            }
        }

        public void TakeScreenshot(string fileName = null)
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
