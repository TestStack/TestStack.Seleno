using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class ShoppingCart : Page
    {
        public AddressAndPaymentPage Checkout()
        {
            return Navigate().To<AddressAndPaymentPage>(By.LinkText("Checkout >>"));
        }
    }
}