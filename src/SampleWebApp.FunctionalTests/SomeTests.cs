using NUnit.Framework;
using OpenQA.Selenium;
using SampleWebApp.Models;

namespace SampleWebApp.FunctionalTests
{
    [TestFixture]
    public class SomeTests
    {
        [Test]
        public void CanLogon()
        {
            var homePage = Application
                .HomePage
                .GoToLogonPage()
                .GotToRegisterPage()
                .Register(new RegisterModel { UserName = "Mehdi", Email = "Mehdi@Khalili.com", Password = "123456", ConfirmPassword = "123456"});

            homePage.AssertThatElements(By.LinkText("Log On")).DoNotExist();
            homePage.AssertThatElements(By.LinkText("Log Off")).Exist();
        }
    }
}