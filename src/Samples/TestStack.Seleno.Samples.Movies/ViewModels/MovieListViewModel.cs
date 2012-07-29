using System;
using System.ComponentModel.DataAnnotations;

namespace TestStack.Seleno.Samples.Movies.ViewModels
{
    public class MovieListViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(5)]
        public string Rating { get; set; }
    }
}