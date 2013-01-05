using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.PageObjects;

namespace TestStack.Samples.ModelIoTesting.PageObjects
{
    public class HomePage : Page
    {
        public HomePage()
        {
            Browser.Navigate().GoToUrl(((SelenoApplication)SelenoApplicationRunner.Host).WebServer.BaseUrl);
        }

        public Form1Page GoToReadModelPage()
        {
            return Navigate().To<Form1Page>(By.LinkText("Fixture A values"));
        }

        public Form1Page GoToWriteModelPage()
        {
            return Navigate().To<Form1Page>(By.LinkText("Empty form, but expecting Fixture A upon submit"));
        }
    }
}