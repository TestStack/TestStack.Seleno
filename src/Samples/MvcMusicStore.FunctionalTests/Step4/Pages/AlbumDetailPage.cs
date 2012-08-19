using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class AlbumDetailPage : Page<Album>
    {
        public ShoppingCartPage AddToCart()
        {
            return Navigate().To<ShoppingCartPage>(By.LinkText("Add to cart"));
        }
    }
}