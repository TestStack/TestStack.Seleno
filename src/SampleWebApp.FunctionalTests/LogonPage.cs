using OpenQA.Selenium;
using SeleniumExtensions;

namespace SampleWebApp.FunctionalTests
{
    public class LogonPage : UiComponent
    {
        public RegisterPage GotToRegisterPage()
        {
            return NavigateTo<RegisterPage>(By.LinkText("Register"));
        }
    }
}