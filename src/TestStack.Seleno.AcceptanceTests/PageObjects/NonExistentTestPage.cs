using System;
using System.Linq;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.PageObjects
{
    public class NonExistentTestPage : Page
    {
        public NonExistentTestPage NavigateToNonExistentPageWithElementFinder()
        {
            Find.Element(By.Name("doesntexist"));
            return this;
        }

        public NonExistentTestPage NavigateToNonExistentPageWithPageNavigator()
        {
            Navigate.To<Page>(By.Id("doesntexist"));
            return this;
        }
    }
}