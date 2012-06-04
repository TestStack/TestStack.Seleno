using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class GenreNavigationComponent : UiComponent
    {
        public AlbumBrowsePage SelectGenreByName(AlbumGenre genre)
        {
            Navigate(By.LinkText(genre.ToString()));
            return PageFactory.Create<AlbumBrowsePage>();
        }
    }
}