using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class HomePage : Page
    {
        public HomePage()
        {
            //NavigateTo<HomePage>(string.Empty);
            _menu = PageFactory.Create<NavigationComponent>();
            _genreMenu = PageFactory.Create<GenreNavigationComponent>();
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
