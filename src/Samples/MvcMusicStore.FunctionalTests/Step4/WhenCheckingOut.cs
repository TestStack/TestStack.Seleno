using FluentAssertions;
using MvcMusicStore.FunctionalTests.Step4.Pages;

namespace MvcMusicStore.FunctionalTests.Step4
{
    class WhenCheckingOut : CheckoutScenario
    {
        AlbumDetailPage _albumDetailPage;
        ShoppingCart _shoppingCartPage;

        // should be straight to the db, not through the UI
        public void AndGiven_I_have_an_album_in_my_Cart()
        {
            AlbumBrowsePage albumBrowsePage = HomePage.GenreMenu.SelectGenreByName(AlbumGenre.Disco);
            _albumDetailPage = albumBrowsePage.SelectAlbumByName("Le Freak");
        }

        // should be straight to the db, not through the UI
        public void AndGiven_I_am_on_the_Shopping_Cart_page()
        {
             _shoppingCartPage = _albumDetailPage.AddToCart();
        }

        public void When_I_Select_checkout()
        {
            _shoppingCartPage.Checkout();
        }

        public void Then_I_will_be_taken_to_the_Address_and_Payment_page()
        {
            Browser.Title.Should().Be("Address And Payment");
        }

        protected override string ScenarioTitle
        {
            get { return "Checking out with valid address and payment information"; }
        }
    }
}