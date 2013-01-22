using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects
{
    [TestFixture]
    public class ScriptAndReturn_specification : SpecificationFor<ScriptExecutor>
    {
        private const string ExecutedScript = @"$('#id').is(':visible')";
        private readonly Type _expectedType = typeof(bool);
        private object _result;

        public override void InitialiseSystemUnderTest()
        {
            SUT = new ScriptExecutor(Fake<IWebDriver>(), Fake<IJavaScriptExecutor>(), Fake<IElementFinder>(), Fake<ICamera>());
        }

        public void Given_javascript_executor_returns_true()
        {
            Fake<IJavaScriptExecutor>()
                .ExecuteScript(Arg.Any<string>())
                .Returns("true");
        }

        public void When_executing_ScriptAndReturn()
        {
            _result = SUT.ScriptAndReturn(ExecutedScript, _expectedType);
        }

        public void Then_it_should_have_executed_the_given_javascript()
        {
            Fake<IJavaScriptExecutor>().Received().ExecuteScript("return " + ExecutedScript);
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