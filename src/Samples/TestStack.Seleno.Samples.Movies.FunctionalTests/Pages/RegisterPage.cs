using TestStack.Seleno.PageObjects;
using OpenQA.Selenium;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests.Pages
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