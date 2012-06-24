using TestStack.Seleno.PageObjects;

using OpenQA.Selenium;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class NavigationComponent : UiComponent
    {
        public LogonPage SelectAdminForNotLoggedInUser()
        {
            return NavigateTo<LogonPage>(By.LinkText("Admin"));
        }
    }
}