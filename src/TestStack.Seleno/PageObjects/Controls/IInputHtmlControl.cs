using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IInputHtmlControl : IHTMLControl
    {
        TReturn ValueAs<TReturn>();
        InputType Type { get; }

        void ReplaceInputValueWith<TProperty>(TProperty inputValue);
    }
}