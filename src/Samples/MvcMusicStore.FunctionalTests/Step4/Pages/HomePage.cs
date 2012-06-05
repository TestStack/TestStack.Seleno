using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class HomePage : Page
    {
        public HomePage()
        {
            _menu = GetComponent<NavigationComponent>();
            _genreMenu = GetComponent<GenreNavigationComponent>();
        }

        NavigationComponent _menu;
        public NavigationComponent Menu
        {
            get { return _menu; }
        }

        GenreNavigationComponent _genreMenu;
        public GenreNavigationComponent GenreMenu
        {
            get { return _genreMenu; }
        }

    }
}
