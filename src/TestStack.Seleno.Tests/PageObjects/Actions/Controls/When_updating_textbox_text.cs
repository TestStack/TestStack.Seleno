using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    internal class When_updating_textbox_text : HtmlControlSpecificationFor<TextBox, string>
    {
        private const string Text = "textbox content";

        public When_updating_textbox_text() : base(x => x.Name)
        {
        }

        public void When_updating_the_textbox_value()
        {
            SUT.Text = Text;
        }

        public void Then_script_executor_should_execute_relevant_script_to_replace_the_value()
        {
            Executor
                .Received()
                .Script(string.Format("$('[name=\"Name\"]').val(\"{0}\")", Text));
        }
    }
}
