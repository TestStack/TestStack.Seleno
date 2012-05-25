using OpenQA.Selenium;
using Seleno;
using Seleno.PageObjects;

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
