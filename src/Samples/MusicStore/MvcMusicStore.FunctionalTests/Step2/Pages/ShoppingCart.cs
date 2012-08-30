using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class ShoppingCart : Page
    {
        public AddressAndPaymentPage Checkout()
        {
            return Navigate().To<AddressAndPaymentPage>(By.LinkText("Checkout >>"));
        }
    }
}