namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ICheckBox : IHtmlControl
    {
        bool Checked { get; set; }
    }
    
    public class CheckBox : InputHtmlControl, ICheckBox
    {
        public const string CheckedAttribute = "checked";

        public bool Checked
        {
            get { return AttributeValueAs<object>(CheckedAttribute) != null; }
            set { AddOrRemoveAttribute(CheckedAttribute, value); }
        }
    }
}