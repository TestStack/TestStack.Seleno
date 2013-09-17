using By = TestStack.Seleno.PageObjects.Locators.By;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.PageObjects.Controls
{
    public class TextArea : HTMLControl
    {
        public virtual string Content
        {
            get
            {
                return Find.Element(By.Id(Id)).GetAttribute("value");
            }
            set
            {
                var scriptToExecute = string.Format(@"$(""#{0}"").text(""{1}"")", Id, value.ToJavaScriptString());
                Execute.Script(scriptToExecute);
            }
        }
    }
}
