using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class AddressAndPaymentPage : Page
    {
        public string FirstName { set { Execute(By.Name("FirstName"), e => e.ClearAndSendKeys(value)); } }
        public string LastName { set { Execute(By.Name("LastName"), e => e.ClearAndSendKeys(value)); } }
        public string Address { set { Execute(By.Name("Address"), e => e.ClearAndSendKeys(value)); } }
        public string City { set { Execute(By.Name("City"), e => e.ClearAndSendKeys(value)); } }
        public string State { set { Execute(By.Name("State"), e => e.ClearAndSendKeys(value)); } }
        public string PostalCode { set { Execute(By.Name("PostalCode"), e => e.ClearAndSendKeys(value)); } }
        public string Country { set { Execute(By.Name("Country"), e => e.ClearAndSendKeys(value)); } }
        public string Phone { set { Execute(By.Name("Phone"), e => e.ClearAndSendKeys(value)); } }
        public string Email { set { Execute(By.Name("Email"), e => e.ClearAndSendKeys(value)); } }
        public string PromoCode { set { Execute(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }

        public Page SubmitOrder()
        {
            return NavigateTo<Page>(By.CssSelector("input[type=submit]"));
        }
    }
}