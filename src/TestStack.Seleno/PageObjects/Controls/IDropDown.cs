namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IDropDown : ISelectableHtmlControl
    {
        string SelectedElementText { get; }
        void SelectElementByText(string optionText);
    }
}