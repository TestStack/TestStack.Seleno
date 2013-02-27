using System.Web.Mvc;
using FluentAssertions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class Input_Type_of_TextBox_Is_Text : HtmlControlSpecificationFor<TextBox>
    {
        public Input_Type_of_TextBox_Is_Text() : base(x => x.Modified) { }
        
        public void Then_the_Input_Type_should_be_Text()
        {
            SUT.Type.Should().Be(InputType.Text);
        }
    }
}