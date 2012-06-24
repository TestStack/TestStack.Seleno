using MvcMusicStore.Models;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class AlbumBrowsePage : Page<Genre>
    {
        public AlbumDetailPage SelectAlbumByName(string name)
        {
            string selector = string.Format("img[alt=\"{0}\"]", name);
            return NavigateTo<AlbumDetailPage>(By.CssSelector(selector));
        }
    }
}