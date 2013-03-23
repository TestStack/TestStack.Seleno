using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects
{
    [TestFixture]
    class ScriptAndReturnSpecification : SpecificationFor<Executor>
    {
        private const string ExecutedScript = @"$('#id').is(':visible')";
        private readonly Type _expectedType = typeof(bool);
        private object _result;

        public void Given_javascript_executor_returns_true()
        {
            SubstituteFor<IJavaScriptExecutor>()
                .ExecuteScript("return " + ExecutedScript)
                .Returns("true");
        }

        public void When_executing_ScriptAndReturn()
        {
            _result = SUT.ScriptAndReturn(ExecutedScript, _expectedType);
        }

        public void Then_it_should_have_executed_the_given_javascript()
        {
            SubstituteFor<IJavaScriptExecutor>().Received().ExecuteScript("return " + ExecutedScript);
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