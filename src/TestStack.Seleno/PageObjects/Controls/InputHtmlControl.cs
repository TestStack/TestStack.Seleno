using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public abstract class InputHtmlControl : HTMLControl
    {
        public string Value => ValueAs<string>();

        public virtual TReturn ValueAs<TReturn>()
        {
            var scriptToExecute = $@"$('[name=""{Name}""]').val()";
            return Execute.ScriptAndReturn<TReturn>(scriptToExecute);
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            var scriptToExecute = $@"$('[name=""{Name}""]').val(""{inputValue.ToString().ToJavaScriptString()}"")";
            Execute.Script(scriptToExecute);
        }
    }
}