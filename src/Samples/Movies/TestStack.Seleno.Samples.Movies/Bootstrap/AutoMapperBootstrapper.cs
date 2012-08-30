using TestStack.Seleno.Samples.Movies.Core.Domain;
using TestStack.Seleno.Samples.Movies.ViewModels;

using AutoMapper;

namespace TestStack.Seleno.Samples.Movies.Bootstrap
{
    public static class AutoMapperBootstrapper
    {
        public static void Configure()
        {
            Mapper.CreateMap<Movie, MovieListViewModel>();
            Mapper.AssertConfigurationIsValid();
        }
    }
}