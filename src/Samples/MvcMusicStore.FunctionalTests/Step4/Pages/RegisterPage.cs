using MvcMusicStore.Models;
using TestStack.Seleno.PageObjects;

using OpenQA.Selenium;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class RegisterPage : Page<RegisterModel>
    {
        public HomePage CreateValidUser(RegisterModel registerModel)
        {
            FillWithModel(registerModel);
            Navigate(By.CssSelector("input[type=\"submit\"]"));
            return new HomePage();
        }
    }
}