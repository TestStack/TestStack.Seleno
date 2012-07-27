using System;
using System.Linq;
using MvcMovie.Models;

namespace MvcMovie.Infrastructure.Data.InMemory
{
    public class MovieRepository : IMovieRepository
    {
        public MovieListViewModel FindById(Guid id)
        {
            return Db.Movies.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<MovieListViewModel> GetAll()
        {
            return Db.Movies.AsQueryable();
        }

        public MovieListViewModel SaveOrUpdate(MovieListViewModel model)
        {
            var movie = Db.AddMovie(model);
            return movie;
        }
    }
}