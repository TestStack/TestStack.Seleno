using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step1.Pages
{
    public class HomePage : Page
    {
        public LogonPage GoToAdminForAnonymousUser()
        {
            return Navigate().To<LogonPage>(By.LinkText("Admin"));
        }

        public AlbumBrowsePage SelectGenreByName(string genre)
        {
            return Navigate().To<AlbumBrowsePage>(By.LinkText(genre));
        }
    }
}
