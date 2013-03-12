using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    // todo: not sure if I would bother with this tbh
    class Input_type_of_radio_button_group_is_radio : HtmlControlSpecificationFor<RadioButtonGroup>
    {
        public Input_type_of_radio_button_group_is_radio() : base(x => x.Choice) { }

        public void Then_the_input_type_should_be_radio()
        {
            SUT.Type.Should().Be(InputType.Radio);
        }
    }
}