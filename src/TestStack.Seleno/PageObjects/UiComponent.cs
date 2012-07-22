using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Fakes;
using TestStack.Seleno.Specifications.Assertions;
//using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent
    {
        protected internal RemoteWebDriver Browser;

        public UiComponent()
        {
            if(SelenoApplicationRunner.Host != null)
                Browser = SelenoApplicationRunner.Host.Browser as RemoteWebDriver;
        }

        protected TPage NavigateTo<TPage>(By clickDestination)
            where TPage : UiComponent, new()
        {
            Navigate(clickDestination);

            return new TPage {Browser = Browser};
        }

        protected TDestinationPage NavigateTo<TDestinationPage>(string relativeUrl)
            where TDestinationPage : UiComponent, new()
        {
            Browser.Navigate().GoToUrl(relativeUrl);
            return new TDestinationPage {Browser = Browser};
        }

        public ElementAssert AssertThatElements(By selector)
        {
            return new ElementAssert(this, selector);
        }

        protected IWebElement TryFindElement(By by)
        {
             IWebElement result = null;
             try
             {
                 result = Browser.FindElement(by);
             }
             catch (NoSuchElementException)
             {
             }

            return result;
         }

        protected TDestinationPage NavigateTo<TController, TDestinationPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TDestinationPage : UiComponent, new()
        {
            var helper = new HtmlHelper(new ViewContext { HttpContext = FakeHttpContext.Root() }, new FakeViewDataContainer());
            var relativeUrl = helper.BuildUrlFromExpression(action);

            return NavigateTo<TDestinationPage>(IISExpressRunner.HomePage + relativeUrl);
        }

        public TComponent GetComponent<TComponent>()
            where TComponent : UiComponent, new()
        {
            return new TComponent() {Browser = Browser};
        }

        protected void Navigate(By clickDestination)
        {
            Execute(clickDestination, e => e.Click());
        }

        public IWebElement Execute(By findElement, Action<IWebElement> action)
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

        public TResult Execute<TResult>(By findElement, Func<IWebElement, TResult> func)
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

        public IWebElement ExecuteWithPatience(By findElement, Action<IWebElement> action, int waitInSeconds = 20)
        {
            try
            {
                var element = FindElementWithWait(findElement, waitInSeconds);
                action(element);
                return element;
            }
            catch (Exception)
            {
                TakeScreenshot();
                throw;
            }
        }

        IWebElement FindElementWithWait(By findElement, int waitInSeconds = 20)
        {
            var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(waitInSeconds));
            return wait.Until(d => d.FindElement(findElement));
        }

        public TReturn ExecuteScriptAndReturn<TReturn>(string javascriptToBeExecuted)
        {
            var javascriptExecutor = ((IJavaScriptExecutor)Browser);
            return (TReturn)javascriptExecutor.ExecuteScript("return " + javascriptToBeExecuted);
        }

        public void WaitForAjaxCallsToFinish(int timeOutInSecond = 10)
        {
            var stillGoing = true;
            int waitedFor = 0;

            while (stillGoing)
            {
                Thread.Sleep(Configurator.WaitForAjaxPollingInterval);
                waitedFor++;

                stillGoing = !(bool) Browser.ExecuteScript("return jQuery.active == 0");
                if (waitedFor > timeOutInSecond)
                    throw new SelenoException(
                        string.Format("Wait for AJAX timed out after waiting for {0} seconds", timeOutInSecond));
            }
        }

        public void TakeScreenshot(string fileName = null)
        {
            var pathFromConfig = Configurator.ScreenShotPath;
            var camera = (ITakesScreenshot) Browser;
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
