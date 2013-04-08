using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_executing_predicate_script_that_completes_successfuly : ExecutorSpecification
    {
        private const string JqueryIsLoadedScript = "typeof jQuery == 'function'";
        private int _index;
        private readonly bool[] _returnedValues = new [] {false, true, false};

        public void Given_the_predicate_script_will_return_true_only_the_second_time()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript("return " + JqueryIsLoadedScript)
                .Returns(c => _returnedValues[_index++]);
        }

        public void When_executing_predicate_script()
        {
            SUT.PredicateScriptAndWaitToComplete(JqueryIsLoadedScript);
        }

        public void Then_it_should_execute_the_predicate_script_2_times()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .Received(2)
                .ExecuteScript("return " + JqueryIsLoadedScript);
        }
    }
}