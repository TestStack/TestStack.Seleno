using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_setting_an_attribute_value_for_any_element : HtmlControlSpecificationFor<TextBox, string>
    {
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue \\ \" \r\n";

        public When_setting_an_attribute_value_for_any_element() : base(x => x.Name)
        {
            SUT.SetAttributeValue(AttributeName, AttributeValue);
        }

        public void Then_it_should_execute_javascript_to_set_attribute()
        {
            Executor
                .Received(1)
                .Script(string.Format(@"$('#Name').attr('{0}', ""someValue \\ \"" \r\n"")", AttributeName));
        }
    }
}