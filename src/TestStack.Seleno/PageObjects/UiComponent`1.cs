using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel : class, new()
    {
        public UiComponent()
        {
            throw new InaccessibleSelenoException();
        }

        internal UiComponent(IWebDriver browser, IPageNavigator pageNavigator, IElementFinder elementFinder,
            IScriptExecutor scriptExecutor, ICamera camera, IComponentFactory componentFactory)
            : base(browser, pageNavigator, elementFinder, scriptExecutor, camera, componentFactory) { }

        protected PageReader<TModel> Read()
        {
            return ComponentFactory.CreatePageReader<TModel>();
        }

        protected PageWriter<TModel> Input()
        {
            return ComponentFactory.CreatePageWriter<TModel>();
        }

        internal static UiComponent<TModel> Create()
        {
            return new UiComponent<TModel>(null, null, null, null, null, null);
        } 
    }
}