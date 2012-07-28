using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests.Pages.Movies
{
    public class MovieListPage : Page<MovieListViewModel>
    {
        public TableReader<MovieListViewModel> MovieList
        {
            get { return TableFor<MovieListViewModel>("ApplicationsListGrid"); }
        }
    }
}
