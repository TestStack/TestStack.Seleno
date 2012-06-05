using FluentAssertions;
using MvcMusicStore.FunctionalTests.Step4.Pages;
using MvcMusicStore.Models;
using TestStack.BDDfy;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;
using TestStack.BDDfy.Core;
using NUnit.Framework;

namespace MvcMusicStore.FunctionalTests.Step4
{
    [Story(
        Title = "Can buy music online",
        AsA="As a Customer",
        IWant="I want to purchase an album",
        SoThat="So that I can enjoy my music")]
    public class LivingDocumentation 
    {
        protected HomePage HomePage;
        ShoppingCartPage _sut;
        AddressAndPaymentPage _result;

        public LivingDocumentation()
        {
            HomePage = Application.HomePage;
        }

        [Test]
        public void CheckingOutWithValidAddressAndPaymentInformation()
        {
            this.Given(_ => Given_that_I_am_a_logged_in_user())
                .And(_ => AndGiven_I_am_on_the_Shopping_Cart_page())
                .And(_ => AndGiven_I_have_an_album_in_my_Cart())
                .When(_ => When_I_Select_checkout())
                .Then(_ => Then_I_will_be_taken_to_the_Address_and_Payment_page())
                .BDDfy();
        }

        [Test]
        public void WhenEnteringValidAddressAndPaymentInformation()
        {
            this.Given(_ => Given_that_I_am_a_logged_in_user())
                    .And(_ => And_am_checking_out())
                    .And(_ => And_am_entering_Address_and_Payment_information())
                .When(_ => When_I_enter_valid_information())
                .Then(_ => Then_my_checkout_will_succeed())
                    .And(_ => And_I_will_be_taken_to_the_Checkout_Complete_page())
                .BDDfy();
        }

        // should be straight to the db, not through the UI
        public void AndGiven_I_have_an_album_in_my_Cart()
        {
            _sut = HomePage
                .GenreMenu
                .SelectGenreByName(AlbumGenre.Disco)
                .SelectAlbumByName("Le Freak")
                .AddToCart();
        }

        // should be straight to the db, not through the UI
        public void AndGiven_I_am_on_the_Shopping_Cart_page()
        {
            // implemented in previous step
        }

        public void When_I_Select_checkout()
        {
            _result = _sut.Checkout();
        }

        public void Then_I_will_be_taken_to_the_Address_and_Payment_page()
        {
            _result.Title.Should().Be("Address And Payment");
        }

        public void And_am_checking_out()
        {
        }

        public void And_am_entering_Address_and_Payment_information()
        {
        }

        public void When_I_enter_valid_information()
        {
        }

        public void Then_my_checkout_will_succeed()
        {
        }

        public void And_I_will_be_taken_to_the_Checkout_Complete_page()
        {
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

            HomePage                
                .Menu
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage()
                .CreateValidUser(validUser);
        }
    }
}
