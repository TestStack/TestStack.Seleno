using System;
using System.Collections.Generic;
using MvcMovie.Models;

namespace MvcMovie.Infrastructure.Data.InMemory
{
    public class MovieCache 
    {
        public static IList<MovieListViewModel> Load()
        {
            var movies = new List<MovieListViewModel> {  
 
                    new MovieListViewModel { Title = "Minority Report",   
                             ReleaseDate= new DateTime(2002, 7, 4),   
                             Genre="Sci-Fi",  
                             Rating="PG-13",  
                             Price=7.99M},  

               new MovieListViewModel { Title = "The Dark Knight Rises",   
                             ReleaseDate= new DateTime(12,7,20),   
                             Genre="Action",  
                             Rating="R",  
                             Price=10.99M},   

             new MovieListViewModel { Title = "When Harry Met Sally",   
                             ReleaseDate=DateTime.Parse("1989-1-11"),   
                             Genre="Romantic Comedy",  
                             Rating="R",  
                             Price=7.99M},  

                     new MovieListViewModel { Title = "Top Gun ",   
                             ReleaseDate= new DateTime(1986,10,3),   
                             Genre="Action",  
                              Rating="R",  
                             Price=8.99M},   
  
                 new MovieListViewModel { Title = "The Color Purple",   
                             ReleaseDate= new DateTime(1986,8,1),   
                             Genre="Drama",  
                             Rating="R",  
                             Price=9.99M},   

                 new MovieListViewModel { Title = "The Abyss",   
                             ReleaseDate= new DateTime(1989,10,13),   
                             Genre="Sci-Fi",  
                             Rating="R",  
                             Price=7.99M},  

                     new MovieListViewModel { Title = "Gone Baby Gone",   
                             ReleaseDate= new DateTime(2008,6,6),   
                             Genre="Crime",  
                              Rating="R",  
                             Price=8.99M},   
  
               new MovieListViewModel { Title = "The Color of Money",   
                             ReleaseDate= new DateTime(1987,3,6),   
                             Genre="Drama",  
                             Rating="R",  
                             Price=3.99M},   

                     new MovieListViewModel { Title = "Ghostbusters ",   
                             ReleaseDate=DateTime.Parse("1984-3-13"),   
                             Genre="Comedy",  
                              Rating="R",  
                             Price=8.99M},   
  
                 new MovieListViewModel { Title = "Ghostbusters 2",   
                             ReleaseDate=DateTime.Parse("1986-2-23"),   
                             Genre="Comedy",  
                             Rating="R",  
                             Price=9.99M},   

               new MovieListViewModel { Title = "Rio Bravo",   
                             ReleaseDate=DateTime.Parse("1959-4-15"),   
                             Genre="Western",  
                             Rating="R",  
                             Price=3.99M},   

                 new MovieListViewModel { Title = "Gone With the Wind",   
                             ReleaseDate= new DateTime(1940,1,17),   
                             Genre="Romance",  
                             Rating="R",  
                             Price=9.99M}   

             };

            return movies;
        }
    }
}