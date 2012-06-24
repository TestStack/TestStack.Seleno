using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class GenreNavigationComponent : UiComponent
    {
        public AlbumBrowsePage SelectGenreByName(AlbumGenre genre)
        {
            return NavigateTo<AlbumBrowsePage>(By.LinkText(genre.ToString()));
        }
    }
}