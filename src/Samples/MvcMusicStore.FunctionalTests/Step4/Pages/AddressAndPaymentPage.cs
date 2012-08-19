using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class AddressAndPaymentPage : Page<Order>
    {
        public Page SubmitShippingInfo(Order order, string promoCode)
        {
            Input().Model(order);
            PromoCode = promoCode;

            //Order model = ReadModelFromPage();
            //var element = GetElementFor(x => x.Phone);
            ////model.Should().BeSameAs(order);

            return Navigate().To<Page>(By.CssSelector("input[type=submit]"));
        }

        string PromoCode { set { Execute().ActionOnLocator(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }
    }
}