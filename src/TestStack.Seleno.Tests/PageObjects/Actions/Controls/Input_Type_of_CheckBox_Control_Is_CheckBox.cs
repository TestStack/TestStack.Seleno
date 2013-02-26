using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class Input_Type_of_CheckBox_Control_Is_CheckBox : HtmlControlSpecificationFor<CheckBox>
    {
        public Input_Type_of_CheckBox_Control_Is_CheckBox() : base(x => x.Exists) { }

        public void Then_the_Input_Type_should_be_Text()
        {
            SUT.Type.Should().Be(InputType.CheckBox);
        }
    }
}