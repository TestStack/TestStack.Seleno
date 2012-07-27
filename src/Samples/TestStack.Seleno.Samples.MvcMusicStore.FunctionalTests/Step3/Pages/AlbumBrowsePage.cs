using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class AlbumBrowsePage : Page
    {
        public AlbumDetailPage SelectAlbumByName(string name)
        {
            string selector = string.Format("img[alt=\"{0}\"]", name);
            return NavigateTo<AlbumDetailPage>(By.CssSelector(selector));
        }
    }
}