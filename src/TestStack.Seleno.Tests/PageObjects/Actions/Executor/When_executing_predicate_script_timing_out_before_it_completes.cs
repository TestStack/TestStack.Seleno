using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_executing_predicate_script_timing_out_before_it_completes : ExecutorSpecification
    {
        private const string JqueryIsLoadedScript = "typeof jQuery == 'function'";
        private Action _executeTimingOutScriptAction;
        private readonly TimeSpan _timeOutAfterThreeSecond = new TimeSpan(0,0,0,3);

        public void Given_the_predicate_script_will_always_return_false()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript("return " + JqueryIsLoadedScript)
                .Returns(false);
        }
        
        public void When_executing_predicate_script()
        {
            _executeTimingOutScriptAction = () => SUT.PredicateScriptAndWaitToComplete(JqueryIsLoadedScript, _timeOutAfterThreeSecond);
        }       

        public void Then_it_should_throw_a_TimeOut_Exception()
        {
            _executeTimingOutScriptAction
                .ShouldThrow<TimeoutException>()
                .WithMessage("The predicate script took longer than 3 seconds to verify statement");
        }
    }
}