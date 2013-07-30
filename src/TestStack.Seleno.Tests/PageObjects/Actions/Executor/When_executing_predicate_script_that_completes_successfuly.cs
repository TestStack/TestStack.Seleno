using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_executing_predicate_script_that_completes_successfuly : ExecutorSpecification
    {
        private const string ScriptToBeExecuted = "typeof jQuery == 'function'";
        private const string ExecutedScript = "return " + ScriptToBeExecuted;

        public void Given_the_predicate_script_will_return_true_only_the_second_time()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript(ExecutedScript)
                .Returns(false,true,false);
        }

        public void When_executing_predicate_script()
        {
            SUT.PredicateScriptAndWaitToComplete(ScriptToBeExecuted);
        }

        public void Then_it_should_execute_the_predicate_script_2_times()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .Received(2)
                .ExecuteScript(ExecutedScript);
        }
    }
}