using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel : class, new()
    {
        protected IPageReader<TModel> Read()
        {
            return ComponentFactory.CreatePageReader<TModel>();
        }

        protected IPageWriter<TModel> Input()
        {
            return ComponentFactory.CreatePageWriter<TModel>();
        }
    }
}