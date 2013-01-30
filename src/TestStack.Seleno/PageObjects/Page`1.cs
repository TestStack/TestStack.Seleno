namespace TestStack.Seleno.PageObjects
{
    public class Page<TViewModel> : UiComponent<TViewModel> where TViewModel : class, new()
    {
        public string Title
        {
            get
            {
                return Browser.Title;
            }
        }
    }

    public class MyViewModel {}

    public class MyPage : Page<MyViewModel> {}
}