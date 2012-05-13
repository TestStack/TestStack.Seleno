using OpenQA.Selenium;
using SeleniumExtensions;

namespace SampleWebApp.FunctionalTests
{
    public class HomePage : UiComponent
    {
        public LogonPage GoToLogonPage()
        {
            return NavigateTo<LogonPage>(By.LinkText("Log On"));
        }
    }
}
