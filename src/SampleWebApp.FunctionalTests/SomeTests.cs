using NUnit.Framework;
using OpenQA.Selenium;
using SampleWebApp.Models;

namespace SampleWebApp.FunctionalTests
{
    [TestFixture]
    public class SomeTests
    {
        [Test]
        public void AfterRegistrationYouAreReturnedToHomePage()
        {
            var homePage = Application
                .HomePage
                .GoToLogonPage()
                .GotToRegisterPage()
                .Register(new RegisterModel { UserName = "SomeUser", Email = "User@domain.com", Password = "123456", ConfirmPassword = "123456"});

            homePage.AssertThatElements(By.LinkText("Log On")).DoNotExist();
            homePage.AssertThatElements(By.LinkText("Log Off")).Exist();
        }
    }
}