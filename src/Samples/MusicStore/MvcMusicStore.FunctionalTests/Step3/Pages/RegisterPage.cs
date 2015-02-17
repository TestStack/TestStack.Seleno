using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class RegisterPage : Page<RegisterModel>
    {
        public HomePage CreateValidUser(RegisterModel model)
        {
            Input.Model(model, null, GetName<bool>(x => x.LobValue), GetName<string>(x => x.RegistrationCode));
            return Navigate.To<HomePage>(By.CssSelector("input[type='submit']"));
        }
    }
}