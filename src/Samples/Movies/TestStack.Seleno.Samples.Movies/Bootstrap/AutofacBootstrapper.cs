using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TestStack.Seleno.Samples.Movies.Core.Domain;
using TestStack.Seleno.Samples.Movies.Core.Services;
using TestStack.Seleno.Samples.Movies.Core.Services.InMemoryDataProvider;

namespace TestStack.Seleno.Samples.Movies.Bootstrap
{
	public static class AutofacBootstrapper
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(AutofacBootstrapper).Assembly);
            builder.RegisterType<MovieRepository>().As<IRepository<Movie>>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}