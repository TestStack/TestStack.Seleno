using System;
using System.Globalization;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_executing_predicate_script_timing_out_before_it_completes : ExecutorSpecification
    {
        private const string PredicateScriptToBeExecuted = "typeof jQuery == 'function'";
        private const string ExpectedScriptToBeExecuted = "return " + PredicateScriptToBeExecuted;
        private Action _executeTimingOutScriptAction;
        private readonly TimeSpan _timeOutAfterOneMilliSecond = new TimeSpan(0,0,0,0,1);

        public void Given_the_predicate_script_will_always_return_false()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript(ExpectedScriptToBeExecuted)
                .Returns(false);
        }
        
        public void When_executing_predicate_script()
        {
            _executeTimingOutScriptAction = () => SUT.PredicateScriptAndWaitToComplete(PredicateScriptToBeExecuted, _timeOutAfterOneMilliSecond);
        }       

        public void Then_it_should_throw_a_TimeOut_Exception()
        {
            var timing = 0.001d.ToString(CultureInfo.CurrentCulture);

            _executeTimingOutScriptAction
                .Should().Throw<TimeoutException>()
                .WithMessage(string.Format("The predicate script took longer than {0} seconds to verify statement", timing));
        }
    }
}