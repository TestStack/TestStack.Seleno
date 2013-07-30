using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_checking_whether_ajax_calls_are_completed : ExecutorSpecification
    {
        private const string ExpectedAjaxCallsAreCompleteScript = "return $.active == 0";

        public void Given_the_predicate_script_will_return_true_only_the_second_time()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript(ExpectedAjaxCallsAreCompleteScript)
                .Returns(true);
        }

        public void When_executing_predicate_script()
        {
            SUT.WaitForAjaxCallsToComplete();
        }

        public void Then_it_should_execute_the_predicate_script_2_times()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .Received()
                .ExecuteScript(ExpectedAjaxCallsAreCompleteScript);
        }
    }
}