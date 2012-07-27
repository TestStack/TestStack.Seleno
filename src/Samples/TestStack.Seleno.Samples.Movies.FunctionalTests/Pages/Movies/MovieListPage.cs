using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Components;
using TestStack.Seleno.Samples.Movies.Models;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests.Pages.Movies
{
    public class MovieListPage : Page<MovieListViewModel>
    {
        public TableComponent<MovieListViewModel> MovieList
        {
            get { return TableFor<MovieListViewModel>("ApplicationsListGrid"); }
        }
    }
}
