using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests.Pages
{
    public class LogonPage : Page<LogOnModel>
    {
        public RegisterPage GoToRegisterPage()
        {
            return Navigate().To<RegisterPage>(By.LinkText("Register"));
        }

        public TDestinationPage LoginWithValidUserAndNavigateToPage<TController, TDestinationPage>(LogOnModel logonModel, Expression<Action<TController>> action)
            where TController : Controller
            where TDestinationPage : UiComponent, new()
        {
            Input().Model(logonModel);
            Navigate().To(By.CssSelector("input[type=\"submit\"]"));
            return Navigate().To<TController, TDestinationPage>(action);
        }
    }
}