using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class LogonPage : Page
    {
        public RegisterPage GoToRegisterPage()
        {
            return Navigate().To<RegisterPage>(By.LinkText("Register"));
        }
    }
}