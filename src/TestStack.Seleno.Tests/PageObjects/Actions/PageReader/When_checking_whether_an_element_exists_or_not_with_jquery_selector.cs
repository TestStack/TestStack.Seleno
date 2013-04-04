using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Locators;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_checking_an_element_exists_and_is_visible_with_jquery_selector : PageReaderSpecification
    {
        private const string JquerySelector = "a:contains('some text'):first";
        private readonly string _expectedScriptToBeExecuted = string.Format("$(\"{0}\").is(':visible')", JquerySelector);

        public When_checking_an_element_exists_and_is_visible_with_jquery_selector()
        {
            SUT.ExistsAndIsVisible(By.jQuery(JquerySelector));
        }

        public void Then_it_should_execute_the_relevant_script_based_on_jQuery_selector()
        {
            SubstituteFor<IExecutor>()
                .Received()
                .ScriptAndReturn<bool>(_expectedScriptToBeExecuted);
        }
    }
}