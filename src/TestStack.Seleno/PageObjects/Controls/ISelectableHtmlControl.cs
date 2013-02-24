namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ISelectableHtmlControl : IHTMLControl
    {
        bool HasSelectedElement { get; }
        TProperty SelectedElementAs<TProperty>();
        void SelectElement<TProperty>(TProperty value);
    }
}