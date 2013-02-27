using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class Input_Type_of_Radio_button_group_Is_Radio : HtmlControlSpecificationFor<RadioButtonGroup>
    {
        public Input_Type_of_Radio_button_group_Is_Radio() : base(x => x.Choice) { }

        public void Then_the_Input_Type_should_be_Text()
        {
            SUT.Type.Should().Be(InputType.Radio);
        }
    }
}