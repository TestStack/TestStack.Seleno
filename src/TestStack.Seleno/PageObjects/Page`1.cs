namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : UiComponent<TViewModel> where TViewModel : new()
    {
        public string Title
        {
            get
            {
                return Browser.Title;
            }
        }
    }
}