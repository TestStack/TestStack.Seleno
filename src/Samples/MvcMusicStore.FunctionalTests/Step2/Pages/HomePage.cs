using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step2.Pages
{
    public class HomePage : Page
    {
        public LogonPage SelectAdminForNotLoggedInUser()
        {
            return NavigateTo<LogonPage>(By.LinkText("Admin"));
        }

        public AlbumBrowsePage SelectGenreByName(string genre)
        {
            return NavigateTo<AlbumBrowsePage>(By.LinkText(genre));
        }
    }
}
