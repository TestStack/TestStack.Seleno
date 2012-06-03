using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class AlbumDetailPage : Page<Album>
    {
        public ShoppingCart AddToCart()
        {
            Navigate(By.LinkText("Add to cart"));
            return PageFactory.Create<ShoppingCart>();
        }
    }
}