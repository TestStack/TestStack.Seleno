using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : UiComponent<TViewModel> where TViewModel : class, new()
    {
        public string Title
        {
            get
            {
                return Browser.TitleWithWait();
            }
        }
    }
}