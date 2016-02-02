using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects.Locators;

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
                var scriptToExecute = $@"$(""#{Id}"").text(""{value.ToJavaScriptString()}"")";
                Execute.Script(scriptToExecute);
            }
        }
    }
}
