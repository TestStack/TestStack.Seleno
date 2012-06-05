using MvcMusicStore.FunctionalTests.Step4.Pages;
using MvcMusicStore.Models;
using TestStack.BDDfy;
using TestStack.BDDfy.Core;
using TestStack.Seleno.PageObjects;

using NUnit.Framework;

namespace MvcMusicStore.FunctionalTests.Step4
{
    [Story(
        AsA="As a Customer",
        IWant="I want to purchase an album",
        SoThat="So that I can enjoy my music")]
    public abstract class CheckoutScenario 
    {
        protected abstract string ScenarioTitle { get; }

        [Test]
        public void BddifyMe()
        {
            this.BDDfy(ScenarioTitle);
        }

        // should be straight to the db, not through the UI
        public void Given_that_I_am_a_logged_in_user()
        {
            //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
            var validUser = new RegisterModel()
            {
                UserName = "HJSimpson",
                Email = "chunkylover53@aol.com",
                Password = "!2345Qwert",
                ConfirmPassword = "!2345Qwert"
            };

            Application
                .HomePage
                .Menu
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage()
                .CreateValidUser(validUser);
        }
    }
}
