﻿using FluentAssertions;
using MvcMusicStore.SampleFunctionalTests.Pages;
using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.BDDfy.Core;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Scenarios
{
    [Story(
        Title = "Can buy music online",
        AsA = "As a Customer",
        IWant = "I want to purchase an album",
        SoThat = "So that I can enjoy my music")]
    public class when_checking_out
    {
        HomePage _homePage;
        ShoppingCartPage _sut;
        AddressAndPaymentPage _shippingInfoPage;
        Page _resultPage;

        [Test]
        public void CanBuyMusicOnline()
        {
            this.BDDfy();
        }

        // should be straight to the db, not through the UI
        public void Given_that_I_am_a_logged_in_user()
        {
            _homePage = new HomePage(); //SelenoApplication
                //.HomePage
            _homePage
                .Menu
                .SelectAdminForNotLoggedInUser()
                .GoToRegisterPage()
                .CreateValidUser(ObjectMother.CreateRegisterModel());
        }

        // should be straight to the db, not through the UI
        public void AndGiven_I_add_an_album_to_my_Cart()
        {
            _sut = _homePage
                .GenreMenu
                .SelectGenreByName(AlbumGenre.Disco)
                .SelectAlbumByName("Le Freak")
                .AddToCart();
        }

        public void When_I_Checkout()
        {
            _shippingInfoPage = _sut.Checkout();
        }

        public void AndWhen_I_submit_my_shipping_info()
        {
            _resultPage = _shippingInfoPage.SubmitShippingInfo(ObjectMother.CreateShippingInfo(), "FREE");
        }

        public void Then_my_order_will_be_completed()
        {
        }

        public void And_I_will_be_taken_to_the_Checkout_Complete_page()
        {
            _resultPage.Title.Should().Be("Checkout Complete");
        }
    }
}
