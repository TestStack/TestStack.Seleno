using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    // todo: not sure if I would bother with this tbh
    class Input_type_of_textbox_is_text : HtmlControlSpecificationFor<TextBox>
    {
        public Input_type_of_textbox_is_text() : base(x => x.Modified) { }
        
        public void Then_the_input_type_should_be_text()
        {
            SUT.Type.Should().Be(InputType.Text);
        }
    }
}