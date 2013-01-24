using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Tests.PageObjects.Actions.ScriptExecutor
{
    public class when_failing_at_performing_action_on_locator : ScriptExecutorSpecification
    {
        private readonly Action<IWebElement> _actionOnWebElement = e => { throw new Exception("Can't do this"); };
        private readonly Action _failingActionOnLocator;
       

        public when_failing_at_performing_action_on_locator()
        {
            _failingActionOnLocator = () => SUT.ActionOnLocator(By.Id("id"), _actionOnWebElement);
        }
        
        public void Then_it_should_throw_the_original_exception()
        {
            _failingActionOnLocator
                .ShouldThrow<Exception>()
                .WithMessage("Can't do this");
        }

        public void AndThen_it_should_take_a_SnapShot()
        {
            Fake<ICamera>().Received().TakeScreenshot();
        }
    }
}