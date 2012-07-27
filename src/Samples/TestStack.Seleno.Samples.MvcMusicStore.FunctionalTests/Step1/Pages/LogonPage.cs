using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class LogonPage : Page
    {
        public RegisterPage GoToRegisterPage()
        {
            return NavigateTo<RegisterPage>(By.LinkText("Register"));
        }
    }
}