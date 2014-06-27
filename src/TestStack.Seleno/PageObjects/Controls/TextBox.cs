namespace TestStack.Seleno.PageObjects.Controls
{
    public class TextBox : InputHtmlControl
    {
        public string Text
        {
            get { return ValueAs<string>(); }
            set { ReplaceInputValueWith(value);}
        }

    }
}
