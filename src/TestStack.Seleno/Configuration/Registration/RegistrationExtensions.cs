using Autofac;
using Autofac.Builder;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Assertions;

namespace TestStack.Seleno.Configuration.Registration
{
    internal static class RegistrationExtensions
    {
        internal static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> OnActivatedInitialiseUiComponent<TLimit, TActivatorData, TRegistrationStyle>
            (this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
        {
            return registration.OnActivated(e =>
                {
                    var component = e.Instance as UiComponent;
                    if (component == null)
                        return;

                    component.Browser = e.Context.Resolve<IWebDriver>();
                    component.Camera = e.Context.Resolve<ICamera>();
                    component.ComponentFactory = e.Context.Resolve<IComponentFactory>();
                    component.ElementFinder = e.Context.Resolve<IElementFinder>();
                    component.PageNavigator = e.Context.Resolve<IPageNavigator>();
                    component.Executor = e.Context.Resolve<IExecutor>();
                    component.ElementAssert = e.Context.Resolve<IElementAssert>();
                    component.Wait = e.Context.Resolve<IWait>();
                }
            );
        }

        internal static void RegisterProxiedClass<TConcrete, TInterface>(this ContainerBuilder builder)
            where TConcrete : TInterface
            where TInterface : class
        {
            builder.RegisterType<TConcrete>()
                .SingleInstance();
            builder.Register(CreateCameraProxyFor<TConcrete, TInterface>)
                .SingleInstance();
        }

        private static TInterface CreateCameraProxyFor<TConcrete, TInterface>(IComponentContext c)
            where TConcrete : TInterface
            where TInterface : class
        {
            return c.Resolve<ProxyGenerator>().CreateInterfaceProxyWithTarget<TInterface>(c.Resolve<TConcrete>(), new CameraProxyInterceptor(c.Resolve<ICamera>(), typeof(TConcrete).Name, c.Resolve<ILoggerFactory>().Create(typeof(TConcrete))));
        }
    }
}