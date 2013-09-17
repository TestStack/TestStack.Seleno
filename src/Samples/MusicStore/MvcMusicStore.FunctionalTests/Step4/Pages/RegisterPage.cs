using MvcMusicStore.Models;
using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class RegisterPage : Page<RegisterModel>
    {
        public HomePage CreateValidUser(RegisterModel registerModel)
        {
            Input.Model(registerModel);
            return Navigate.To<HomePage>(By.CssSelector("input[type=\"submit\"]"));
        }
    }
}