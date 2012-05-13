using System.Threading;
using NUnit.Framework;
using SampleWebApp.Models;

namespace SampleWebApp.FunctionalTests
{
    [TestFixture]
    public class SomeTests
    {
        [Test]
        public void CanLogon()
        {
            Application
                .HomePage
                .GoToLogonPage()
                .GotToRegisterPage()
                .Register(new RegisterModel { UserName = "Mehdi", Email = "Mehdi@Khalili.com", Password = "123456", ConfirmPassword = "123456"})
                .TakeScreenshot("d:\\temp\\end page.png");
        }
    }
}