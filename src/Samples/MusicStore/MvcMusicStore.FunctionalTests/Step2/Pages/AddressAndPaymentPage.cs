using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class AddressAndPaymentPage : Page
    {
        public string FirstName { set { Execute().ActionOnLocator(By.Name("FirstName"), e => e.ClearAndSendKeys(value)); } }
        public string LastName { set { Execute().ActionOnLocator(By.Name("LastName"), e => e.ClearAndSendKeys(value)); } }
        public string Address { set { Execute().ActionOnLocator(By.Name("Address"), e => e.ClearAndSendKeys(value)); } }
        public string City { set { Execute().ActionOnLocator(By.Name("City"), e => e.ClearAndSendKeys(value)); } }
        public string State { set { Execute().ActionOnLocator(By.Name("State"), e => e.ClearAndSendKeys(value)); } }
        public string PostalCode { set { Execute().ActionOnLocator(By.Name("PostalCode"), e => e.ClearAndSendKeys(value)); } }
        public string Country { set { Execute().ActionOnLocator(By.Name("Country"), e => e.ClearAndSendKeys(value)); } }
        public string Phone { set { Execute().ActionOnLocator(By.Name("Phone"), e => e.ClearAndSendKeys(value)); } }
        public string Email { set { Execute().ActionOnLocator(By.Name("Email"), e => e.ClearAndSendKeys(value)); } }
        public string PromoCode { set { Execute().ActionOnLocator(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }

        public Page SubmitOrder()
        {
            return Navigate().To<Page>(By.CssSelector("input[type=submit]"));
        }
    }
}