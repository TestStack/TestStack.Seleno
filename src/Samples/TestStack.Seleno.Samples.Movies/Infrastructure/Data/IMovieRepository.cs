using System;
using System.Linq;

using MvcMovie.Models;

namespace MvcMovie.Infrastructure.Data
{
    public interface IMovieRepository
    {
        MovieListViewModel FindById(Guid id);
        IQueryable<MovieListViewModel> GetAll();
        MovieListViewModel SaveOrUpdate(MovieListViewModel model);
    }
}