using MvcMusicStore.ViewModels;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class ShoppingCart : Page<ShoppingCartViewModel>
    {
        public void Checkout()
        {
            Navigate(By.LinkText("Checkout >>"));
        }
    }
}