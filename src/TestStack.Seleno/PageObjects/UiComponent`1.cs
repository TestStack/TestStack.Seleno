using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.PageObjects
{
    public class UiComponent<TModel> : UiComponent
        where TModel: class, new()
    {
        protected PageReader<TModel> Read()
        {
            return new PageReader<TModel>(Browser, _executor, _finder);
        }

        protected PageWriter<TModel> Input()
        {
            return new PageWriter<TModel>(Browser, _executor, _finder);
        } 
    }
}