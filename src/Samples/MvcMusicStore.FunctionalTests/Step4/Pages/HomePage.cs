using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class HomePage : Page
    {
        private NavigationComponent _menu;
        public NavigationComponent Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = GetComponent<NavigationComponent>();                    
                }
                return _menu;
            }
        }

        private GenreNavigationComponent _genreMenu;
        public GenreNavigationComponent GenreMenu
        {
            get
            {
                if (_genreMenu == null)
                {
                    _genreMenu = GetComponent<GenreNavigationComponent>();
                }
                return _genreMenu;
            }
        }

    }
}
