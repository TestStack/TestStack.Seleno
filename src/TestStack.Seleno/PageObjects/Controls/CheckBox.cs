using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ICheckBox : IHtmlControl
    {
        bool Checked { get; set; }
    }
    
    public class CheckBox : InputHtmlControl, ICheckBox
    {
        public const string CheckedAttribute = "checked";
        
        public override InputType Type
        {
            get { return InputType.CheckBox; }
        }

        public bool Checked
        {
            get { return AttributeValueAs<object>(CheckedAttribute) != null; }
            set { AddOrRemoveAttribute(CheckedAttribute, value); }
        }
    }
}