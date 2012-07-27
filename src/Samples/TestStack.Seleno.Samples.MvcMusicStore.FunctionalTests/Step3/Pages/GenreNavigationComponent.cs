using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class GenreNavigationComponent : UiComponent
    {
        public AlbumBrowsePage SelectGenreByName(string genre)
        {
            return NavigateTo<AlbumBrowsePage>(By.LinkText(genre));
        }
    }
}