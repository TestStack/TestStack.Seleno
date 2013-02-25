using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ITextBox : IInputHtmlControl { }

    public class TextBox : InputHtmlControl, ITextBox
    {
        public override InputType Type
        {
            get { return InputType.Text;}
        }
    }
}
