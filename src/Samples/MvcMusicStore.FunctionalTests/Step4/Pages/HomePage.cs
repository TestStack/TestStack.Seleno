using TestStack.Seleno.PageObjects;

namespace MvcMusicStore.FunctionalTests.Step4.Pages
{
    public class HomePage : Page
    {
        public HomePage()
        {
            //NavigateTo<HomePage>(string.Empty);
            _menu = PageFactory.Create<NavigationComponent>();
        }

        NavigationComponent _menu;
        public NavigationComponent Menu
        {
            get { return _menu; }
        }
    }
}
