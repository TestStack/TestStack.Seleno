using System;
using System.Collections.Generic;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.Infrastructure.Data.InMemory
{
    public static class Db
    {
        static IList<MovieListViewModel> _movies;
        public static IList<MovieListViewModel> Movies
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

        public static MovieListViewModel AddMovie(MovieListViewModel movieList)
        {
            movieList.Id = Guid.NewGuid();
            _movies.Add(movieList);
            return movieList;
        }
    }
}