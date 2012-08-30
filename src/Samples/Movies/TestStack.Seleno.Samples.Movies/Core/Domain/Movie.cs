using System;

namespace TestStack.Seleno.Samples.Movies.Core.Domain
{
    public class Movie : Entity
    {
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public string Rating { get; set; }
    }
}