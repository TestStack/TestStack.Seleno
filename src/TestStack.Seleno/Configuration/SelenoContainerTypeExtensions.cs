using System;
using System.Collections.Generic;
using System.Reflection;
using Funq;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    public static class SelenoContainerTypeExtensions
    {
        // todo: Should it be RegisterComponents rather than PageObjects given it's both?
        private static void RegisterPageObject<T>(Container container, ReuseScope scope = ReuseScope.None)
            where T : UiComponent, new()
        {
            var registration = container.Register(c => new T()
            {
                Browser = c.Resolve<IWebDriver>(),
                Camera = c.Resolve<ICamera>(),
                ComponentFactory = c.Resolve<IComponentFactory>(),
                ElementFinder = c.Resolve<IElementFinder>(),
                PageNavigator = c.Resolve<IPageNavigator>(),
                ScriptExecutor = c.Resolve<IScriptExecutor>()
            });

            registration.ReusedWithin(scope);
        }

        public static void RegisterPageObjects(this Container container, IEnumerable<Type> serviceTypes, ReuseScope scope = ReuseScope.None)
        {
            var method = typeof(SelenoContainerTypeExtensions).GetMethod("RegisterPageObject",
                BindingFlags.Static | BindingFlags.NonPublic
            );

            foreach (var serviceType in serviceTypes)
            {
                var invocation = method.MakeGenericMethod(serviceType);
                invocation.Invoke(null, new object[]{container, scope});
            }
        }
    }
}