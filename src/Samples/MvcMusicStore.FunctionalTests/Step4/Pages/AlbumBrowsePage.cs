using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class AlbumBrowsePage : Page<Genre>
    {
        public AlbumDetailPage SelectAlbumByName(string name)
        {
            string selector = string.Format("img[alt=\"{0}\"]", name);
            Navigate(By.CssSelector(selector));
            return PageFactory.Create<AlbumDetailPage>();
        }
    }
}