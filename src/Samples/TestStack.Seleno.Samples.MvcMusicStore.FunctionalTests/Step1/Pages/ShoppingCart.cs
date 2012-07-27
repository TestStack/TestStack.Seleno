using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class ShoppingCart : Page
    {
        public AddressAndPaymentPage Checkout()
        {
            return NavigateTo<AddressAndPaymentPage>(By.LinkText("Checkout >>"));
        }
    }
}