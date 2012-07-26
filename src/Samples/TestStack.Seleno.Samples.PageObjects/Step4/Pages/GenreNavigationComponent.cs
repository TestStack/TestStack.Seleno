using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.PageObjects.Step4.Pages
{
    public class GenreNavigationComponent : UiComponent
    {
        public AlbumBrowsePage SelectGenreByName(AlbumGenre genre)
        {
            return NavigateTo<AlbumBrowsePage>(By.LinkText(genre.ToString()));
        }
    }
}