using OpenQA.Selenium;
using SampleWebApp.Controllers;
using Seleno;

namespace SampleWebApp.FunctionalTests
{
    public class LogonPage : UiComponent
    {
        public RegisterPage GotToRegisterPage()
        {
            //return NavigateTo<RegisterPage>(By.LinkText("Register"));
            return NavigateTo<AccountController, RegisterPage>(x => x.Register());
        }
    }
}