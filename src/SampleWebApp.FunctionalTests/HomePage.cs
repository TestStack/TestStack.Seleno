using OpenQA.Selenium;
using Seleno;

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
