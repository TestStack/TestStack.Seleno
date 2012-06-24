using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class AddressAndPaymentPage : Page<Order>
    {
        public Page SubmitShippingInfo(Order order, string promoCode)
        {
            FillWithModel(order);
            PromoCode = promoCode;
            return NavigateTo<Page>(By.CssSelector("input[type=submit]"));
        }

        string PromoCode { set { Execute(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }
    }
}