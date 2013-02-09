using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    public class When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver : PageWriterSpecification
    {
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";
        
        public When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver()
        {
            SUT.SetAttribute(x => x.Name, AttributeName, AttributeValue);
        }

        public void Then_it_should_execute_javascript_to_set_attribute()
        {
            SubstituteFor<IScriptExecutor>()
                .Received(1)
                .ExecuteScript(string.Format("$('#Name').attr('{0}','{1}'))", AttributeName, AttributeValue));

        }
    }
}