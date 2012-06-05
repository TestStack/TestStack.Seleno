using MvcMusicStore.Models;
using NUnit.Framework;
using FluentAssertions;

namespace MvcMusicStore.FunctionalTests.Step2
{
    public class StronglyTypedPageObject
    {
        [Test]
        public void Can_buy_an_Album_when_registered()
        {
            var user = CreateRegisterModel();
            var shippingInfo = CreateShippingInfo();
            var orderedPage = Application
                .HomePage
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage()
                .CreateValidUser(user)
                .SelectGenreByName("Disco")
                .SelectAlbumByName("Le Freak")
                .AddAlbumToCart()
                .Checkout()
                .SubmitShippingInfo(shippingInfo, "Free");

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
