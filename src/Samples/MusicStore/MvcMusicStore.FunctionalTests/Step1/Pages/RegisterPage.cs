using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class RegisterPage : Page
    {
        public HomePage SubmitRegistration()
        {
            return Navigate().To<HomePage>(By.CssSelector("input[type='submit']"));
        }

        public string Username          { set { Execute().ActionOnLocator(By.Name("UserName"), e => e.ClearAndSendKeys(value)); } }
        public string Email             { set { Execute().ActionOnLocator(By.Name("Email"), e => e.ClearAndSendKeys(value)); } }
        public string ConfirmPassword   { set { Execute().ActionOnLocator(By.Name("ConfirmPassword"), e => e.ClearAndSendKeys(value)); } }
        public string Password          { set { Execute().ActionOnLocator(By.Name("Password"), e => e.ClearAndSendKeys(value)); } }
    }
}