using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_updating_multiline_content_of_textarea : HtmlControlSpecificationFor<TextArea>
    {
        private const string Content = "line 1\nline 2\nnew line 3";

        public When_updating_multiline_content_of_textarea() : base(x => x.MultiLineContent)
        {
            SUT.Content = Content;
        }

        public void Then_script_executor_should_execute_script_to_update_multiline_content()
        {
            Executor
                .Received()
                .Script(@"$(""#MultiLineContent"").text(""line 1\nline 2\nnew line 3"")");
        }
        
    }
}