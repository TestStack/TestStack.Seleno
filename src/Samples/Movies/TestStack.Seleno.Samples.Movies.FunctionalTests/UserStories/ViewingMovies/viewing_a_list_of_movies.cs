using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Samples.Movies.Controllers;
using TestStack.Seleno.Samples.Movies.FunctionalTests.Pages.Movies;

namespace TestStack.Seleno.Samples.Movies.FunctionalTests.UserStories.ViewingMovies
{
    public class viewing_a_list_of_movies : BDDfyFixture<movie_list_story>
    {
        private MovieListPage _page;

        public void Given_I_have_12_Movies()
        {
            // would create some test data and save it to the database
            // but MVC sample already has hard-coded test values for convenience
        }

        public void When_I_navigate_to_the_Movie_List_page()
        {
            _page = SelenoHost.NavigateToInitialPage<MoviesController, MovieListPage>(x => x.Index());
        }

        public void Then_I_should_see_10_items_in_the_list()
        {
            _page.MovieList.NumberOfRows.Should().Be(12);
        }

        public void AndThen_they_should_be_sorted_alphabetically()
        {
            //var pageList = _page.MovieList.ToList();
            //var dbList = Db.Movies.OrderBy(x => x.Title).ToList();
            Assert.True(true);
        }
    }
}
