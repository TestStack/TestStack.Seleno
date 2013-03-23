using OpenQA.Selenium;
using TestStack.Seleno.AcceptanceTests.Web.Controllers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Web.PageObjects
{
    public class HomePage : Page
    {
        public Form1Page GoToReadModelPage()
        {
            return Navigate().To<Form1Page>(By.LinkText("Fixture A values"));
        }

        public Form1Page GoToWriteModelPage()
        {
            return Navigate().To<Form1Page>(By.LinkText("Empty form, but expecting Fixture A upon submit"));
        }

        public Form1Page GoToReadModelPageByUrl()
        {
            return Navigate().To<Form1Page>("/Form1/FixtureA");
        }

        public Form1Page GoToReadModelPageByMvcAction()
        {
            return Navigate().To<Form1Controller, Form1Page>(c => c.FixtureA());
        }

        public Form1Page GoToReadModelPageByLink()
        {
            return GoToReadModelPage();
        }

        public Form1Page GoToReadModelPageByButton()
        {
            return Navigate().To<Form1Page>(By.CssSelector("input[type=submit]"));
        }
    }
}