using NUnit.Framework;
using FluentAssertions;
using TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3
{
    public class StronglyTypedPageObjectWithComponent
    {
        [Test]
        public void Can_buy_an_Album_when_registered()
        {
            var orderedPage = new HomePage()
                .Menu
                .GoToAdminForAnonymousUser()
                .GoToRegisterPage()
                .CreateValidUser(ObjectMother.CreateRegisterModel())
                .GenreMenu
                .SelectGenreByName("Disco")
                .SelectAlbumByName("Le Freak")
                .AddAlbumToCart()
                .Checkout()
                .SubmitShippingInfo(ObjectMother.CreateShippingInfo(), "Free");

            orderedPage.Title.Should().Be("Checkout Complete");
        }
    }
}
