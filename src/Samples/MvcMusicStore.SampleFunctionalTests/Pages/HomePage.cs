using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.SampleFunctionalTests.Pages
{
    public class HomePage : Page
    {
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
