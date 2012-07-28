using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Components;
using TestStack.Seleno.Specifications.Assertions;
using By = OpenQA.Selenium.By;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal RemoteWebDriver Browser;
        protected IPageNavigator _navigator;
        protected IScriptExecutor _executor;
        protected IElementFinder _finder;

        public UiComponent()
        {
            if (SelenoApplicationRunner.Host != null)
                Browser = SelenoApplicationRunner.Host.Browser as RemoteWebDriver;
        }

        protected IPageNavigator Navigate()
        {
            return new PageNavigator(Browser, new ScriptExecutor(Browser, new ElementFinder(Browser)));
        }

        protected IScriptExecutor Execute()
        {
            return new ScriptExecutor(Browser, new ElementFinder(Browser));
        }

        protected IElementFinder Find()
        {
            return new ElementFinder(Browser);
        }

        protected TableComponent<TModel> TableFor<TModel>(string gridId) where TModel : class, new()
        {
            return new TableComponent<TModel>(gridId) { Browser = Browser };
        }

        public ElementAssert AssertThatElements(By selector)
        {
            return new ElementAssert(this, selector);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            return new TComponent() { Browser = Browser };
        }

        //protected IWebElement TryFindElement(By by)
        //{
        //    IWebElement result = null;
        //    try
        //    {
        //        result = Browser.FindElement(by);
        //    }
        //    catch (NoSuchElementException)
        //    {
        //    }

        //    return result;
        //}

        //protected IWebElement TryFindElement(Locators.By.jQueryBy by)
        //{
        //    IWebElement result = null;
        //    try
        //    {
        //        result = Browser.FindElementByjQuery(by);
        //    }
        //    catch (NoSuchElementException)
        //    {
        //    }

        //    return result;
        //}

        //IWebElement FindElementWithWait(By findElement, int waitInSeconds = 20)
        //{
        //    var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(waitInSeconds));
        //    return wait.Until(d => d.FindElement(findElement));
        //}

        //public TReturn ExecuteScriptAndReturn<TReturn>(string javascriptToBeExecuted)
        //{
        //    var javascriptExecutor = ((IJavaScriptExecutor)Browser);
        //    return (TReturn)javascriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
        //}

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
