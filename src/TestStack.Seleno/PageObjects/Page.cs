using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects
{
    public class Page : UiComponent
    {
        public string Title
        {
            get { return Browser.TitleWithWait(); }
        }

        public string Url { get { return Browser.Url; } }
    }
}