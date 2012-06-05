using FluentAssertions;
using NUnit.Framework;

namespace MvcMusicStore.FunctionalTests.Step3
{
    public class StronglyTypedPageObjectWithComponents
    {
        [Test]
        public void Can_buy_an_Album_when_registered()
        {
            var homepage = Application.HomePage;

            var orderedPage = homepage
                .Menu
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage()
                .CreateValidUser(ObjectMother.CreateRegisterModel())
                .GenreMenu
                .SelectGenreByName("Disco")
                .SelectAlbumByName("Le Freak")
                .AddToCart()
                .Checkout()
                .SubmitShippingInfo(ObjectMother.CreateShippingInfo(), "FREE");

            orderedPage.Title.Should().Be("Checkout Complete");
        }
    }
}
