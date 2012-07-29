using MvcMusicStore.Models;
using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class RegisterPage : Page<RegisterModel>
    {
        public HomePage CreateValidUser(RegisterModel model)
        {
            Input().Model(model);
            return Navigate().To<HomePage>(By.CssSelector("input[type='submit']"));
        }
    }
}