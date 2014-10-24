using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using TestStack.Seleno.AcceptanceTests.Web.Controllers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public class HomePage : Page
    {
        public Form1Page GoToReadModelPage()
        {
            return Navigate.To<Form1Page>(By.LinkText("Fixture A values"));
        }

        public DetailsPage GoToDetailsPage()
        {
            return Navigate.To<DetailsPage>(By.LinkText("Details Page"));
        }

        public Form1Page GoToWriteModelPage()
        {
            return Navigate.To<Form1Page>(By.LinkText("Empty form, but expecting Fixture A upon submit"));
        }

        public Form1Page GoToReadModelPageByRelativeUrl()
        {
            return Navigate.To<Form1Page>("/Form1/FixtureA");
        }

        public Form1Page GoToReadModelPageByAbsoluteUrl()
        {
            return Navigate.To<Form1Page>(Host.Instance.Application.WebServer.BaseUrl + "/Form1/FixtureA");
        }

        public Form1Page GoToReadModelPageByMvcAction()
        {
            return Navigate.To<Form1Controller, Form1Page>(c => c.FixtureA());
        }

        public Form1Page GoToReadModelPageByLink()
        {
            return GoToReadModelPage();
        }

        public Form1Page GoToReadModelPageByButton()
        {
            return Navigate.To<Form1Page>(By.CssSelector("input[type=submit]"));
        }

        public ListPage GoToListPage()
        {
            return Navigate.To<HomeController, ListPage>(c => c.List());
        }

        public FormWithAJAXPage GoToFormWithAjax()
        {
            return Navigate.To<FormWithAJAXPage>("/Form1/FormWithAJAX");
        }

        public HomePage SelectCheckingEnvironmentVariables()
        {
            return Navigate.To<HomePage>(By.LinkText("Checking Environment Variables"));
        }
    }

    public class ListPage : Page
    {
        public IEnumerable<IWebElement> Items { get { return Find.Elements(By.CssSelector("#ul li")); } }
        public IEnumerable<IWebElement> ItemsByJQuery { get { return Find.Elements(Seleno.PageObjects.Locators.By.jQuery("#ul li")); } }

        public IEnumerable<IWebElement> FindNonExistantItems(TimeSpan maxWait)
        {
            return Find.Elements(By.ClassName("nonexistant"), maxWait);
        }

        public IEnumerable<IWebElement> FindNonExistantItemsByJQuery(TimeSpan maxWait)
        {
            return Find.Elements(Seleno.PageObjects.Locators.By.jQuery(".nonexistant"), maxWait);
        }
    }
}