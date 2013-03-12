using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface IInputHtmlControl : IHtmlControl
    {
        string Value { get; }
        TReturn ValueAs<TReturn>();
        InputType Type { get; }

        void ReplaceInputValueWith<TProperty>(TProperty inputValue);
    }

    public abstract class InputHtmlControl : HTMLControl, IInputHtmlControl
    {
        public abstract InputType Type { get; }

        public string Value
        {
            get { return ValueAs<string>(); }
        }
        
        public TReturn ValueAs<TReturn>()
        {
            var scriptToExecute = string.Format("$('#{0}').val()", Id);
            return Execute().ScriptAndReturn<TReturn>(scriptToExecute);
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            var scriptToExecute = string.Format("$('#{0}').val('{1}')", Id, inputValue);
            Execute().ExecuteScript(scriptToExecute);
        }
    }
}