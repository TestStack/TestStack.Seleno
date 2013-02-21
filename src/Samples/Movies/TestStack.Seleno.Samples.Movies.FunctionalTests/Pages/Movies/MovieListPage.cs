using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Samples.Movies.ViewModels;

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
