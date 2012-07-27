using OpenQA.Selenium.Remote;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step3.Pages
{
    public class HomePage : Page
    {
        public RemoteWebDriver Driver
        {
            get
            {
                return Browser;
            }
        }

        public NavigationComponent Menu
        {
            get { return GetComponent<NavigationComponent>(); }
        }

        public GenreNavigationComponent GenreMenu
        {
            get { return GetComponent<GenreNavigationComponent>(); }
        }
    }
}
