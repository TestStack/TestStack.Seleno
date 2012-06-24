using MvcMusicStore.ViewModels;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class ShoppingCartPage : Page<ShoppingCartViewModel>
    {
        public AddressAndPaymentPage Checkout()
        {
            return NavigateTo<AddressAndPaymentPage>(By.LinkText("Checkout >>"));
        }
    }
}