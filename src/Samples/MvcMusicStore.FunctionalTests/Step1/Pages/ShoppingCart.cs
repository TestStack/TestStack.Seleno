using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class ShoppingCart : Page
    {
        public Page Checkout()
        {
            return NavigateTo<Page>(By.LinkText("Checkout >>"));
        }
    }
}