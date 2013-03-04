using Autofac;
using Autofac.Builder;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    internal static class RegistrationExtensions
    {
        internal static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnActivatedInitialiseUiComponent<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return
                registration.OnActivated(e =>
                                             {
                                                 var component = e.Instance as UiComponent;
                                                 if (component != null)
                                                 {
                                                     component.Browser = e.Context.Resolve<IWebDriver>();
                                                     component.Camera = e.Context.Resolve<ICamera>();
                                                     component.ComponentFactory = e.Context.Resolve<IComponentFactory>();
                                                     component.ElementFinder = e.Context.Resolve<IElementFinder>();
                                                     component.PageNavigator = e.Context.Resolve<IPageNavigator>();
                                                     component.ScriptExecutor = e.Context.Resolve<IScriptExecutor>();
                                                 }
                                             });
        }
    }
}