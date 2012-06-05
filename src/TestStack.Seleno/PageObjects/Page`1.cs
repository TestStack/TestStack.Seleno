namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : UiComponent<TViewModel>
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