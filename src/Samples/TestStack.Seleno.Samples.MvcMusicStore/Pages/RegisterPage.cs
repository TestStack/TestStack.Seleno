using MvcMusicStore.Models;
using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;

namespace TestStack.Seleno.Samples.MvcMusicStore.Pages
{
    public class RegisterPage : Page<RegisterModel>
    {
        public HomePage CreateValidUser(RegisterModel registerModel)
        {
            FillWithModel(registerModel);
            return NavigateTo<HomePage>(By.CssSelector("input[type=\"submit\"]"));
        }
    }
}