using System.Linq;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using By = TestStack.Seleno.PageObjects.Locators.By;

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
            get
            {
                var textAreaElement = Find().ElementWithWait(By.Id(Id));

                return
                    textAreaElement
                        .Text
                        .Split('\n')
                        .Select(line => line.Replace("\r", string.Empty))
                        .ToArray();
            }
            set
            {
                Execute().ExecuteScript(string.Format("$('#{0}').text('{1}')", Id, string.Join("\n", value)));
            }
        }
    }
}
