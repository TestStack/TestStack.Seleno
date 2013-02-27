namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ITextArea : IHtmlControl
    {
        string[] MultiLineContent { get; set; }
    }

    public class TextArea : HTMLControl, ITextArea
    {
        public string[] MultiLineContent
        {
            get { return Execute().ScriptAndReturn<string[]>(string.Format("$('#{0}').text().split('\n')", Id)); }
            set { Execute().ExecuteScript(string.Format("$('#{0}').text('{1}')", Id, string.Join("\n", value))); }
        }
    }
}
