using System;
using System.Linq;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.Infrastructure.Data.InMemory
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