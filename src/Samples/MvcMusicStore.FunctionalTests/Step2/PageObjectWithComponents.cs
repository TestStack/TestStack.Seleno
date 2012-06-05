using FluentAssertions;
using MvcMusicStore.FunctionalTests.Step2.Pages;
using NUnit.Framework;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2
{
    public class PageObjectWithComponents
    {
        [Test]
        public void Can_buy_an_Album_when_registered()
        {
            var homepage = Application.HomePage;

            var registerPage = homepage
                .Menu
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage();

            registerPage.Username = "HJSimpson";
            registerPage.Email = "chunkylover53@aol.com";
            registerPage.Password = "!2345Qwert";
            registerPage.ConfirmPassword = "!2345Qwert";

            homepage = registerPage.SubmitRegistration();
            var orderedPage = PlaceOrder(homepage);
            orderedPage.Title.Should().Be("Checkout Complete");

            orderedPage.Title.Should().Be("Checkout Complete");
        }

        private static Page PlaceOrder(HomePage homepage)
        {
            var shippingPage = homepage
                .GenreMenu
                .SelectGenreByName("Disco")
                .SelectAlbumByName("Le Freak")
                .AddAlbumToCart()
                .Checkout();

            shippingPage.FirstName = "Homer";
            shippingPage.LastName = "Simpson";
            shippingPage.Address = "742 Evergreen Terrace";
            shippingPage.City = "Springfield";
            shippingPage.State = "Kentucky";
            shippingPage.PostalCode = "123456";
            shippingPage.Country = "United States";
            shippingPage.Phone = "2341231241";
            shippingPage.Email = "chunkylover53@aol.com";
            shippingPage.PromoCode = "FREE";
            return shippingPage.SubmitOrder();
        }
    }
}
