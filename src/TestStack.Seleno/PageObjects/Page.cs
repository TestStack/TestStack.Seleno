namespace TestStack.Seleno.PageObjects
{
    public class Page : UiComponent
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