using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Mvc3ToolsUpdateWeb_Default.Models;

using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class LogonPage : Page<LogOnModel>
    {
        public RegisterPage GoToRegisterPage()
        {
            Navigate(By.LinkText("Register"));
            //Browser.FindElement(By.LinkText("Register")).Click();
            return PageFactory.Create<RegisterPage>();
        }

        public TDestinationPage LoginWithValidUserAndNavigateToPage<TController, TDestinationPage>(LogOnModel logonModel, Expression<Action<TController>> action)
            where TController : Controller
            where TDestinationPage : UiComponent, new()
        {
            FillWithModel(logonModel);
            Navigate(By.CssSelector("input[type=\"submit\"]"));
            return NavigateTo<TController, TDestinationPage>(action);
        }

    }
}