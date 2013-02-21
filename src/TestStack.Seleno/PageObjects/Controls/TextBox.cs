using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class TextBox : InputHtmlControl
    {
        public override InputType Type
        {
            get { return InputType.Text;}
        }
    }
}
