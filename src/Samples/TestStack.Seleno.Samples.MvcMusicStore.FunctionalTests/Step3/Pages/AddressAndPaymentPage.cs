﻿using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class AddressAndPaymentPage : Page<Order>
    {
        public Page SubmitShippingInfo(Order order, string promoCode)
        {
            Input().Model(order);
            PromoCode = promoCode;
            return Navigate().To<Page>(By.CssSelector("input[type=submit]"));
        }

        string PromoCode { set { Execute().ActionOnLocator(By.Name("PromoCode"), e => e.ClearAndSendKeys(value)); } }
    }
}