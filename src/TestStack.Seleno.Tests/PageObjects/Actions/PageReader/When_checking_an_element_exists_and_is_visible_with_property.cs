using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_checking_an_element_exists_and_is_visible_with_property : PageReaderSpecification
    {
        public When_checking_an_element_exists_and_is_visible_with_property()
        {
            SUT.ExistsAndIsVisible(x => x.Item);
        }

        public void Then_it_should_execute_the_relevant_script_with_jquery_id_selector()
        {
            SubstituteFor<IExecutor>()
                .Received()
                .ScriptAndReturn<bool>("$(\"#Item\").is(':visible')");
        }
    }
}