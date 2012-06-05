using MvcMusicStore.FunctionalTests.Step2.Pages;
using MvcMusicStore.Models;
using NUnit.Framework;
using FluentAssertions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2
{
    public class StronglyTypedPageObject
    {
            [Test]
            public void Can_buy_an_Album_when_registered()
            {
                var model = CreateRegisterModel();
                var shippingInfo = CreateShippingInfo();
                var homepage = Application.HomePage;

                var registerPage = homepage
                    .SelectAdminForNotLoggedInUser()
                    .GoToRegisterPage();
                registerPage.FillWithModel(model);
                homepage = registerPage.SubmitRegistration();

                var shippingPage = homepage
                    .SelectGenreByName("Disco")
                    .SelectAlbumByName("Le Freak")
                    .AddToCart()
                    .Checkout();

                shippingPage.FillWithModel(shippingInfo);
                shippingPage.PromoCode = "FREE";
                var orderedPage = shippingPage.SubmitOrder();

                orderedPage.Title.Should().Be("Checkout Complete");
            }

        private static Order CreateShippingInfo()
        {
            var shippingInfo = new Order
                                   {
                                       FirstName = "Homer",
                                       LastName = "Simpson",
                                       Address = "742 Evergreen Terrace",
                                       City = "Springfield",
                                       State = "Kentucky",
                                       PostalCode = "123456",
                                       Country = "United States",
                                       Phone = "2341231241",
                                       Email = "chunkylover53@aol.com"
                                   };
            return shippingInfo;
        }

        private static RegisterModel CreateRegisterModel()
        {
            var model = new RegisterModel
                            {
                                UserName = "HJSimpson",
                                Email = "chunkylover53@aol.com",
                                Password = "!2345Qwert",
                                ConfirmPassword = "!2345Qwert"
                            };
            return model;
        }
    }
}
