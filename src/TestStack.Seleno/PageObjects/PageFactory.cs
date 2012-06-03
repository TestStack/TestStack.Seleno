using OpenQA.Selenium.Remote;

namespace TestStack.Seleno.PageObjects
{
    public static class PageFactory
    {
        private static RemoteWebDriver _browser;

        public static void Initialize(RemoteWebDriver browser)
        {
            _browser = browser;
        }

        public static TDestinationPage Create<TDestinationPage>()
                    where TDestinationPage : UiComponent, new()
        {
            return new TDestinationPage { Browser = _browser };
        }
    }
}
