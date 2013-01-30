using System;
using System.Collections.Generic;
using Funq;

namespace TestStack.Seleno.Configuration
{
    public static class SelenoContainerTypeExtensions
    {
        public static void RegisterPageObject(this Container container, Type serviceType,
                                              ReuseScope scope = ReuseScope.None)
        {
            // This method needs to do the equivalent of this.....

            //container.Register<MyPage>(c => new MyPage()
            //{
            //    Browser = c.Resolve<IWebDriver>(),
            //    Camera = c.Resolve<ICamera>(),
            //    ComponentFactory = c.Resolve<IComponentFactory>(),
            //    ElementFinder = c.Resolve<IElementFinder>(),
            //    PageNavigator = c.Resolve<IPageNavigator>(),
            //    ScriptExecutor = c.Resolve<IScriptExecutor>()
            //});

            //Don't try to register base service classes
            if (serviceType.IsAbstract || serviceType.ContainsGenericParameters)
                return;

            var methodInfo = typeof(Container).GetMethod("Register", Type.EmptyTypes);
            var registerMethodInfo = methodInfo.MakeGenericMethod(new[] { serviceType });

            var registration = registerMethodInfo.Invoke(container, new object[0]) as IRegistration;
            registration.ReusedWithin(scope);
        }

        public static void RegisterPageObjects(this Container container, IEnumerable<Type> serviceTypes,
                                               ReuseScope scope = ReuseScope.None)
        {
            foreach (var serviceType in serviceTypes)
                container.RegisterPageObject(serviceType, scope);
        }
    }
}