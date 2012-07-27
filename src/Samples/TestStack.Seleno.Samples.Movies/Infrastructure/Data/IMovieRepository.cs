using System;
using System.Linq;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.Infrastructure.Data
{
    public interface IMovieRepository
    {
        MovieListViewModel FindById(Guid id);
        IQueryable<MovieListViewModel> GetAll();
        MovieListViewModel SaveOrUpdate(MovieListViewModel model);
    }
}