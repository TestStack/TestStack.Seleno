using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects
{
    public class Page : UiComponent
    {
        public string Title => Browser.TitleWithWait();

        public string Url => Browser.Url;
    }
}