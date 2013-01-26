using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : UiComponent<TViewModel> where TViewModel : class, new()
    {
        public Page()
        {
            throw new InaccessibleSelenoException();
        }

        internal Page(IWebDriver browser, IPageNavigator pageNavigator, IElementFinder elementFinder,
            IScriptExecutor scriptExecutor, ICamera camera, IComponentFactory componentFactory)
            : base(browser, pageNavigator, elementFinder, scriptExecutor, camera, componentFactory) { }

        public string Title
        {
            get
            {
                return Browser.Title;
            }
        }
    }
}