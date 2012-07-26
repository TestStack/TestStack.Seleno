using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;

namespace TestStack.Seleno.Samples.PageObjects.Step2.Pages
{
    public class RegisterPage : Page
    {
        public HomePage SubmitRegistration()
        {
            return NavigateTo<HomePage>(By.CssSelector("input[type='submit']"));
        }

        public string Username { set { Execute(By.Name("UserName"), e => e.ClearAndSendKeys(value)); } }
        public string Email { set { Execute(By.Name("Email"), e => e.ClearAndSendKeys(value)); } }
        public string ConfirmPassword { set { Execute(By.Name("ConfirmPassword"), e => e.ClearAndSendKeys(value)); } }
        public string Password { set { Execute(By.Name("Password"), e => e.ClearAndSendKeys(value)); } }
    }
}