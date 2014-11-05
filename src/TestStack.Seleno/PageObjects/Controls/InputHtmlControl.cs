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
            var scriptToExecute = string.Format(@"$('[name=""{0}""').val()", Name);
            return Execute.ScriptAndReturn<TReturn>(scriptToExecute);
        }

        public void ReplaceInputValueWith<TProperty>(TProperty inputValue)
        {
            var scriptToExecute = string.Format(@"$('[name=""{0}""]').val(""{1}"")", Name, inputValue.ToString().ToJavaScriptString());
            Execute.Script(scriptToExecute);
        }
    }
}