using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace SampleWebApp.FunctionalTests
{
    public class HomePage : Page
    {
        public LogonPage GoToLogonPage()
        {
            return NavigateTo<LogonPage>(By.LinkText("Log On"));
        }
    }
}
