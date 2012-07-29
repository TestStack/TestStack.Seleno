using System;
using System.Collections.Generic;

using TestStack.Seleno.Samples.Movies.Core.Domain;

namespace TestStack.Seleno.Samples.Movies.Core.Services.InMemoryDataProvider
{
    public static class Db
    {
        static IList<Movie> _movies;
        public static IList<Movie> Movies
        {
            get
            {
                if (_movies == null)
                {
                    _movies = MovieCache.Load();
                }

                return _movies;
            }
        }

        public static Movie AddMovie(Movie movieList)
        {
            movieList.Id = Guid.NewGuid();
            _movies.Add(movieList);
            return movieList;
        }
    }
}