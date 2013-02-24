using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    [Ignore("AutoSubstitue cannot resolve TextArea, works with any other controls")]
    class When_retrieving_multiLine_content_of_TextArea : HtmlControlSpecificationFor<TextArea>
    {
        private string[] _result;

        public When_retrieving_multiLine_content_of_TextArea() : base(x => x.MultiLineContent) { }

        public void Given_a_TextArea_has_multi_Lines_of_content()
        {
            ScriptExecutor
                .ScriptAndReturn<string[]>(Arg.Any<string>())
                .Returns(new[] {"line 1", "line 2"});
        }

        public void When_retrieving_the_TextArea_content()
        {
            _result = SUT.MultiLineContent;
        }

        public void Then_script_executor_should_execute_script_to_retrieve_multiline_content()
        {
            ScriptExecutor
                .Received()
                .ScriptAndReturn<string[]>("$('#MultiLineContent').text().split('\n')");
        }

        public void AndThen_it_should_return_the_content_lines_in_the_correct_order()
        {
            _result.Should().ContainInOrder(new [] {"line 1", "line 2"});
        }
    }
}
