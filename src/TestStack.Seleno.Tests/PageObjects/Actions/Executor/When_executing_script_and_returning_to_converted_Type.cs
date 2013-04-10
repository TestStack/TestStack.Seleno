using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Executor
{
    class When_executing_script_and_returning_to_converted_Type : ExecutorSpecification
    {
        private const string ScriptToBeExecuted = @"$('#id').is(':visible')";
        private const string ExecutedScript = "return " + ScriptToBeExecuted;
        private readonly Type _expectedType = typeof(bool);
        private object _result;

        public void Given_javascript_executor_returns_true()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript(ExecutedScript)
                .Returns("true");
        }

        public void When_executing_ScriptAndReturn()
        {
            _result = SUT.ScriptAndReturn(ScriptToBeExecuted, _expectedType);
        }

        public void Then_it_should_have_executed_the_given_javascript()
        {
            SubstituteFor<IJavaScriptExecutor>().Received().ExecuteScript(ExecutedScript);
        }

        public void And_it_should_cast_the_return_type_to_the_specified_type()
        {
            _result.Should().BeOfType<bool>();
        }

        public void And_the_return_value_should_be_correct()
        {
            _result.Should().Be(true);
        }
    }
}