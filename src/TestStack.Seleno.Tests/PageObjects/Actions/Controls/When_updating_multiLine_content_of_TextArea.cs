using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NSubstitute.Core;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_updating_multiLine_content_of_TextArea : HtmlControlSpecificationFor<TextArea>
    {
        public When_updating_multiLine_content_of_TextArea() : base(x => x.MultiLineContent)
        {
            SUT.MultiLineContent = new[] { "line 1", "line 2", "new line 3" };
        }

        public void Then_script_executor_should_execute_script_to_update_multiline_content()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript("$('#MultiLineContent').text('line 1\\nline 2\\nnew line 3')");
                               
        }
        
    }
}