using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_selecting_radio_button_in_radio_group : HtmlControlSpecificationFor<RadioButtonGroup>
    {
        public When_selecting_radio_button_in_radio_group() : base(x => x.Choice)
        {
            SUT.SelectElement(ChoiceType.Another);
        }

        public void Then_it_should_throw_NoSuchElementException()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript("$('input[type=radio][name=Choice][value]')" +
                               ".filter(function() {return $(this).attr('value').toLowerCase() == 'Another'.toLowerCase()})"+
                               ".attr('checked',true)");
        }
    }
}