using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class AddressAndPaymentPage : Page<Order>
    {
        public Page SubmitOrder()
        {
            return NavigateTo<Page>(By.CssSelector("input[type=submit]"));
        }

        public string PromoCode { set { Execute(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }
    }
}