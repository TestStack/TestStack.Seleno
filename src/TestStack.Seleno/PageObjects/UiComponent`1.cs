using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel : class, new()
    {
        protected PageReader<TModel> Read()
        {
            return ComponentFactory.CreatePageReader<TModel>();
        }

        protected PageWriter<TModel> Input()
        {
            return ComponentFactory.CreatePageWriter<TModel>();
        }
    }
}