using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class AlbumDetailPage : Page
    {
        public ShoppingCart AddToCart()
        {
            return Navigate().To<ShoppingCart>(By.LinkText("Add to cart"));
        }
    }
}