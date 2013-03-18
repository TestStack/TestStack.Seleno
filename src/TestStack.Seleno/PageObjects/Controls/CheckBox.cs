namespace TestStack.Seleno.PageObjects.Controls
{
    public class CheckBox : InputHtmlControl
    {
        public const string CheckedAttribute = "checked";

        public virtual bool Checked
        {
            get { return AttributeValueAs<object>(CheckedAttribute) != null; }
            set { AddOrRemoveAttribute(CheckedAttribute, value); }
        }
    }
}