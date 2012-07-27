using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class AlbumDetailPage : Page
    {
        public ShoppingCart AddToCart()
        {
            return NavigateTo<ShoppingCart>(By.LinkText("Add to cart"));
        }
    }
}