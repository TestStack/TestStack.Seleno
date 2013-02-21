using System;
using System.Web.Mvc;

namespace TestStack.Seleno.PageObjects.Controls
{
    public abstract class InputHtmlControl : HTMLControl, IInputHtmlControl
    {
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

        public abstract InputType Type { get; }
    }
}