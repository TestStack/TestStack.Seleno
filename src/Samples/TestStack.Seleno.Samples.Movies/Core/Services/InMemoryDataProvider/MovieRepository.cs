using System;
using System.Linq;

using TestStack.Seleno.Samples.Movies.Core.Domain;

namespace TestStack.Seleno.Samples.Movies.Core.Services.InMemoryDataProvider
{
    public class MovieRepository : IRepository<Movie>
    {
        public Movie FindById(Guid id)
        {
            return Db.Movies.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Movie> GetAll()
        {
            return Db.Movies.AsQueryable();
        }

        public Movie SaveOrUpdate(Movie model)
        {
            var movie = Db.AddMovie(model);
            return movie;
        }
    }
}