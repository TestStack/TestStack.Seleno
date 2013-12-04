using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_getting_whether_checkBox_is_ticked : HtmlControlSpecificationFor<CheckBox, bool>
    {
        private readonly bool _result;

        public void Given_the_checkbox_is_not_ticked() { }

        public When_getting_whether_checkBox_is_ticked() : base(x => x.Exists)
        {
            _result = SUT.Checked;
        }
        
        public void Then_control_should_execute_relevant_script_to_verify_existence_of_checked_attribute()
        {
            Executor
                .Received()
                .ScriptAndReturn<object>("$('#Exists').attr('checked')");
        }

        public void AndThen_it_should_return_false()
        {
            _result.Should().BeFalse();
        }
    }
}