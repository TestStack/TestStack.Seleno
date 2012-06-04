using NUnit.Framework;
using FluentAssertions;

namespace MvcMusicStore.FunctionalTests.Step1
{
    public class BetterTestsWithPageObject
    {
            [Test]
            public void Can_buy_an_Album_when_registered()
            {
                var homepage = Application.HomePage;
                var registerPage = homepage
                    .SelectAdminForNotLoggedInUser()
                    .GoToRegisterPage();

                registerPage.Username = "HGSimpson";
                registerPage.Email = "chunkylover53@aol.com";
                registerPage.Password = "!2345Qwert";
                registerPage.ConfirmPassword = "!2345Qwert";

                homepage = registerPage.SubmitRegistration();
                var orderedPage = homepage
                    .SelectGenreByName("Disco")
                    .SelectAlbumByName("Le Freak")
                    .AddToCart()
                    .Checkout();

                orderedPage.Title.Should().Be("something");
            }
        }
}
