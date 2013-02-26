using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_setting_an_attribute_value_for_any_element : HtmlControlSpecificationFor<TextBox>
    {
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";

        public When_setting_an_attribute_value_for_any_element() : base(x => x.Name)
        {
            SUT.SetAttributeValue(AttributeName, AttributeValue);
        }

        public void Then_it_should_execute_javascript_to_set_attribute()
        {
            ScriptExecutor
                .Received(1)
                .ExecuteScript(string.Format("$('#Name').attr('{0}','{1}')", AttributeName, AttributeValue));
        }


        
    }
}