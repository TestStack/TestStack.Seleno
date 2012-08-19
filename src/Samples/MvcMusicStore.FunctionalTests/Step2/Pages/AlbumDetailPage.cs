using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class AlbumDetailPage : Page
    {
        public ShoppingCart AddAlbumToCart()
        {
            return Navigate().To<ShoppingCart>(By.LinkText("Add to cart"));
        }
    }
}