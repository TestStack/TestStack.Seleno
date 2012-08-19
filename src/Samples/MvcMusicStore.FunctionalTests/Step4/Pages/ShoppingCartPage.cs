using MvcMusicStore.ViewModels;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class ShoppingCartPage : Page<ShoppingCartViewModel>
    {
        public AddressAndPaymentPage Checkout()
        {
            return Navigate().To<AddressAndPaymentPage>(By.LinkText("Checkout >>"));
        }
    }
}