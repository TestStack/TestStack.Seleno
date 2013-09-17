using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class AlbumBrowsePage : Page
    {
        public AlbumDetailPage SelectAlbumByName(string name)
        {
            string selector = string.Format("img[alt=\"{0}\"]", name);
            return Navigate.To<AlbumDetailPage>(By.CssSelector(selector));
        }
    }
}