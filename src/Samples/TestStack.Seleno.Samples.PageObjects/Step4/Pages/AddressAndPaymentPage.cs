using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.PageObjects.Step4.Pages
{
    public class AddressAndPaymentPage : Page<Order>
    {
        public Page SubmitShippingInfo(Order order, string promoCode)
        {
            FillWithModel(order);
            PromoCode = promoCode;

            //Order model = ReadModelFromPage();
            //var element = GetElementFor(x => x.Phone);
            ////model.Should().BeSameAs(order);

            return NavigateTo<Page>(By.CssSelector("input[type=submit]"));
        }

        string PromoCode { set { Execute(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }
    }
}