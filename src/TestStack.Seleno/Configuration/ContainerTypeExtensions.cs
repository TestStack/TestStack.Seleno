using System;
using System.Collections.Generic;
using Funq;

namespace TestStack.Seleno.Configuration
{
    public static class ContainerTypeExtensions
    {
        public static void RegisterAutoWiredType(this Container container, Type serviceType, Type inFunqAsType,
            ReuseScope scope = ReuseScope.None)
        {
            if (serviceType.IsAbstract || serviceType.ContainsGenericParameters)
                return;

            var methodInfo = typeof(Container).GetMethod("RegisterAutoWiredAs", Type.EmptyTypes);
            var registerMethodInfo = methodInfo.MakeGenericMethod(new[] { serviceType, inFunqAsType });

            var registration = registerMethodInfo.Invoke(container, new object[0]) as IRegistration;
            registration.ReusedWithin(scope);
        }

        public static void RegisterAutoWiredType(this Container container, Type serviceType,
            ReuseScope scope = ReuseScope.None)
        {
            //Don't try to register base service classes
            if (serviceType.IsAbstract || serviceType.ContainsGenericParameters)
                return;

            var methodInfo = typeof(Container).GetMethod("RegisterAutoWired", Type.EmptyTypes);
            var registerMethodInfo = methodInfo.MakeGenericMethod(new[] { serviceType });

            var registration = registerMethodInfo.Invoke(container, new object[0]) as IRegistration;
            registration.ReusedWithin(scope);
        }

        public static void RegisterAutoWiredTypes(this Container container, IEnumerable<Type> serviceTypes,
            ReuseScope scope = ReuseScope.None)
        {
            foreach (var serviceType in serviceTypes)
                container.RegisterAutoWiredType(serviceType, scope);
        }
    }
}
