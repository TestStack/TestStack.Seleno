using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class NavigationComponent : UiComponent
    {
        public LogonPage SelectAdminForNotLoggedInUser()
        {
            return NavigateTo<LogonPage>(By.LinkText("Admin"));
        }
    }
}