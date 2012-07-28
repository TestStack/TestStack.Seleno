using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using OpenQA.Selenium;

namespace TestStack.Seleno.PageObjects.Actions
{
    public interface IPageNavigator
    {
        TPage To<TPage>(By clickDestination) where TPage : UiComponent, new();
        TPage To<TPage>(string relativeUrl) where TPage : UiComponent, new();

        TPage To<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new();

        void To(By clickDestination);
    }
}