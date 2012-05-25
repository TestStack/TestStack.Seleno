using OpenQA.Selenium;
using SampleWebApp.Controllers;
using Seleno;
using Seleno.PageObjects;

namespace SampleWebApp.FunctionalTests
{
    public class LogonPage : Page
    {
        public RegisterPage GotToRegisterPage()
        {
            //return NavigateTo<RegisterPage>(By.LinkText("Register"));
            return NavigateTo<AccountController, RegisterPage>(x => x.Register());
        }
    }
}