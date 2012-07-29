using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class GenreNavigationComponent : UiComponent
    {
        public AlbumBrowsePage SelectGenreByName(AlbumGenre genre)
        {
            return Navigate().To<AlbumBrowsePage>(By.LinkText(genre.ToString()));
        }
    }
}