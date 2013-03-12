using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.PageObjects.Controls
{
    public interface ITextArea : IHtmlControl
    {
        string Content { get; set; }
    }

    public class TextArea : HTMLControl, ITextArea
    {
        public string Content
        {
            get
            {
                return Find().ElementWithWait(By.Id(Id)).GetAttribute("value");
            }
            set
            {
                var scriptToExecute = string.Format(@"$(""#{0}"").text(""{1}"")", Id, value.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r"));
                Execute().ExecuteScript(scriptToExecute);
            }
        }
    }
}
