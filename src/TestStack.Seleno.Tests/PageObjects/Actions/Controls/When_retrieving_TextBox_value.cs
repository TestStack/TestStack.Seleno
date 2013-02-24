using System;
using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_retrieving_TextBox_value : HtmlControlSpecificationFor<TextBox>
    {

        public When_retrieving_TextBox_value() : base(x => x.Modified) { }
        

        public void When_retrieving_the_TextBox_value()
        {
            SUT.ValueAs<DateTime>();
        }

        public void Then_script_executor_should_execute_relevant_script_to_retrieve_the_value()
        {
            ScriptExecutor
                .Received()
                .ScriptAndReturn<DateTime>("$('#Modified').val()");

        }
    }
}