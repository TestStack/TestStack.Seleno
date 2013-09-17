using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class AddressAndPaymentPage : Page
    {
        public string FirstName { set { Execute.ActionOnLocator(By.Name("FirstName"), e => { e.Clear(); e.SendKeys(value);}); } }
        public string LastName { set { Execute.ActionOnLocator(By.Name("LastName"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string Address { set { Execute.ActionOnLocator(By.Name("Address"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string City { set { Execute.ActionOnLocator(By.Name("City"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string State { set { Execute.ActionOnLocator(By.Name("State"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string PostalCode { set { Execute.ActionOnLocator(By.Name("PostalCode"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string Country { set { Execute.ActionOnLocator(By.Name("Country"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string Phone { set { Execute.ActionOnLocator(By.Name("Phone"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string Email { set { Execute.ActionOnLocator(By.Name("Email"), e => { e.Clear(); e.SendKeys(value); }); } }
        public string PromoCode { set { Execute.ActionOnLocator(By.Name("PromoCode"), e => { e.Clear(); e.SendKeys(value); }); } }

        public Page SubmitOrder()
        {
            return Navigate.To<Page>(By.CssSelector("input[type=submit]"));
        }
    }
}