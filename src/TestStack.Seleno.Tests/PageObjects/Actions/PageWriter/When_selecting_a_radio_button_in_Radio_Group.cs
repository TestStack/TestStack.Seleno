using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_selecting_a_radio_button_in_Radio_Group : PageWriterSpecification
    {
        public void Given_a_radio_button_group_exists()
        {
            HtmlControl<RadioButtonGroup>(x => x.Choice);
        }
        
        public void When_selecting_a_radio_button()
        {
            SUT.SelectButtonInRadioGroup(x => x.Choice, ChoiceType.Another);
        }

        public void Then_it_should_execute_script_on_drop_down_to_select_option()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript("$('input[type=radio][name=Choice][value=Another]').attr('checked',true)");
        }
    }
}