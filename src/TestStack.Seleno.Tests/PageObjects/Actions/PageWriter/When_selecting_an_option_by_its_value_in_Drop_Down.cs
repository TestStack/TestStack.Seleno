using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_selecting_an_option_by_its_value_in_Drop_Down : PageWriterSpecification
    {

        public void Given_a_drop_down_exists()
        {
            HtmlControl<DropDown>(x => x.Item);
        }
        
        public void When_selecting_an_option_by_its_value()
        {
            SUT.SelectByOptionValueInDropDown(x => x.Item, 2);
        }

        public void Then_it_should_execute_script_on_drop_down_to_select_option()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript("$('#Item').val('2')");
        }
    }
}