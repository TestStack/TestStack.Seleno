using TestStack.Seleno.Samples.Movies.Core.Domain;
using TestStack.Seleno.Samples.Movies.ViewModels;

using AutoMapper;

namespace TestStack.Seleno.Samples.Movies.Bootstrap
{
    public static class AutoMapperBootstrapper
    {
        public static void Configure()
        {
			var config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<Movie, MovieListViewModel>();
				}
			);
			//Mapper.
			config.AssertConfigurationIsValid();
			map = config.CreateMapper();
            //mapper.AssertConfigurationIsValid();
        }

		public static IMapper map;
    }
}