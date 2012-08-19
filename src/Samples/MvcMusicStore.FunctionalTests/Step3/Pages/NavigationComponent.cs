using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class NavigationComponent : UiComponent
    {
        public LogonPage GoToAdminForAnonymousUser()
        {
            return Navigate().To<LogonPage>(By.LinkText("Admin"));
        }
    }
}