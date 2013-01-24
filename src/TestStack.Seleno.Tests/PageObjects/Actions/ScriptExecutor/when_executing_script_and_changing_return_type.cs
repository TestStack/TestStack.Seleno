using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.ScriptExecutor
{
    public class when_executing_script_and_changing_return_type_with_UiComponent : ScriptExecutorSpecification
    {
        private readonly Type _expectedType = typeof(bool);
        private const string executedScript = "$('#id').is(':visible')";
        private object _result;
       
        public void Given_the_script_is_setup_to_return_as_a_string_true()
        {
            Fake<IJavaScriptExecutor>()
                .ExecuteScript(Arg.Any<string>())
                .Returns("true");
        }

        public void When_executing_script_and_changing_return_type()
        {
            _result = SUT.ScriptAndReturn(executedScript, _expectedType);
        }


        public void Then_it_should_execute_given_javascript()
        {
            Fake<IJavaScriptExecutor>().Received().ExecuteScript("return " + executedScript);
        }


        public void AndThen_it_should_cast_the_return_type_to_the_specified_type()
        {
            _result.Should().BeOfType<bool>().And.Be(true);
        }
    }
}