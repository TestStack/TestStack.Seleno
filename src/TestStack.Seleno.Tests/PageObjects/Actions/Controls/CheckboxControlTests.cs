using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    // todo: not sure if I would bother with this tbh
    class Input_type_of_checkbox_control_is_checkbox : HtmlControlSpecificationFor<CheckBox>
    {
        public Input_type_of_checkbox_control_is_checkbox() : base(x => x.Exists) { }

        public void Then_the_input_type_should_be_checkbox()
        {
            SUT.Type.Should().Be(InputType.CheckBox);
        }
    }
}