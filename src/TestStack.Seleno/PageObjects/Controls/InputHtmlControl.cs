using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public abstract class InputHtmlControl : HTMLControl
    {
        public string Value
        {
            get { return ValueAs<string>(); }
        }
        
        public virtual TReturn ValueAs<TReturn>()
        {
            var scriptToExecute = string.Format("$('#{0}').val()", Id);
            return Execute().ScriptAndReturn<TReturn>(scriptToExecute);
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            var scriptToExecute = string.Format(@"$('#{0}').val(""{1}"")", Id, inputValue.ToString().ToJavaScriptString());
            Execute().ExecuteScript(scriptToExecute);
        }
    }
}