using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Builder;
using Autofac.Core;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Configuration.Registration
{
    internal class PageObjectRegistrationSource : IRegistrationSource
    {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            var typedService = service as TypedService;
            if (typedService == null || !typeof(UiComponent).IsAssignableFrom(typedService.ServiceType))
                return Enumerable.Empty<IComponentRegistration>();

            var rb = RegistrationBuilder.ForType(typedService.ServiceType)
                .As(service)
                .InstancePerDependency()
                .OnActivatedInitialiseUiComponent();

            return new[] { rb.CreateRegistration() };
        }

        public bool IsAdapterForIndividualComponents { get { return false; } }
    }
}