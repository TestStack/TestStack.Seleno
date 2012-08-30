using System;
using System.Collections.Generic;

using TestStack.Seleno.Samples.Movies.Core.Domain;

namespace TestStack.Seleno.Samples.Movies.Core.Services.InMemoryDataProvider
{
    public class MovieCache 
    {
        public static IList<Movie> Load()
        {
            var movies = new List<Movie> {  
 
                    new Movie { Title = "Minority Report",   
                             ReleaseDate= new DateTime(2002, 7, 4),   
                             Genre="Sci-Fi",  
                             Rating="PG-13",  
                             Price=7.99M,
                    Id = Guid.NewGuid()},  

               new Movie { Title = "The Dark Knight Rises",   
                             ReleaseDate= new DateTime(12,7,20),   
                             Genre="Action",  
                             Rating="R",  
                             Price=10.99M,
                    Id = Guid.NewGuid()},   

             new Movie { Title = "When Harry Met Sally",   
                             ReleaseDate = new DateTime(1989, 1, 11),   
                             Genre="Romantic Comedy",  
                             Rating="R",  
                             Price=7.99M,
                    Id = Guid.NewGuid()},  

                     new Movie { Title = "Top Gun ",   
                             ReleaseDate= new DateTime(1986,10,3),   
                             Genre="Action",  
                              Rating="R",  
                             Price=8.99M,
                    Id = Guid.NewGuid()},   
  
                 new Movie { Title = "The Color Purple",   
                             ReleaseDate= new DateTime(1986,8,1),   
                             Genre="Drama",  
                             Rating="R",  
                             Price=9.99M,
                    Id = Guid.NewGuid()},   

                 new Movie { Title = "The Abyss",   
                             ReleaseDate= new DateTime(1989,10,13),   
                             Genre="Sci-Fi",  
                             Rating="R",  
                             Price=7.99M,
                    Id = Guid.NewGuid()},  

                     new Movie { Title = "Gone Baby Gone",   
                             ReleaseDate= new DateTime(2008,6,6),   
                             Genre="Crime",  
                              Rating="R",  
                             Price=8.99M,
                    Id = Guid.NewGuid()},   
  
               new Movie { Title = "The Color of Money",   
                             ReleaseDate= new DateTime(1987,3,6),   
                             Genre="Drama",  
                             Rating="R",  
                             Price=3.99M,
                    Id = Guid.NewGuid()},   

                     new Movie { Title = "Meet the Parents",   
                             ReleaseDate= new DateTime(2000, 12, 15),   
                             Genre="Comedy",  
                              Rating="R",  
                             Price=8.99M,
                    Id = Guid.NewGuid()},   
  
                 new Movie { Title = "Meet the Fockers",   
                             ReleaseDate= new DateTime(2004, 11, 18),   
                             Genre="Comedy",  
                             Rating="R",  
                             Price=9.99M,
                    Id = Guid.NewGuid()},   

               new Movie { Title = "Little Fockers",   
                             ReleaseDate = new DateTime(2010, 12, 22),   
                             Genre="Comedy",  
                             Rating="R",  
                             Price=3.99M,
                    Id = Guid.NewGuid()},   

                 new Movie { Title = "Rainman",   
                             ReleaseDate= new DateTime(1988, 11, 11),   
                             Genre="Drama",  
                             Rating="R",  
                             Price=9.99M,
                    Id = Guid.NewGuid()}   

             };

            return movies;
        }
    }
}