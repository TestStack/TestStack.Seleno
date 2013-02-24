using System;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class TextArea : HTMLControl
    {
        public string[] MultiLineContent
        {
            get { return Execute().ScriptAndReturn<String[]>(string.Format("$('#{0}').text().split('\n')", Id)); }
            set { Execute().ExecuteScript(String.Format("$('#{0}').text({1})", Id, String.Join("\n", value))); }
        }

    }
}
