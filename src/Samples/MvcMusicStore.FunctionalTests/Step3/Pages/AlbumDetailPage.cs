using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class AlbumDetailPage : Page
    {
        public ShoppingCart AddToCart()
        {
            return NavigateTo<ShoppingCart>(By.LinkText("Add to cart"));
        }
    }
}