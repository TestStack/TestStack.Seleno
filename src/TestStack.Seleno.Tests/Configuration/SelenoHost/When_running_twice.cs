using System;
using TestStack.Seleno.Configuration;
using SUT = TestStack.Seleno.Configuration.SelenoHost;
using FluentAssertions;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    class When_running_twice : SelenoHostSpecification
    {
        private Exception _caughtException;

        public void Given_the_Seleno_Application_has_already_been_run()
        {
            SUT.Run(c => {});
        }
        
        public void When_running_a_second_time()
        {
            try
            {
                SUT.Run(c => { });
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        public void Then_it_should_have_thrown_an_exception()
        {
            _caughtException.Should().NotBeNull();
        }

        public void And_it_should_be_a_seleno_exception()
        {
            _caughtException.Should().BeOfType<SelenoException>();
        }

        public void And_it_should_have_the_right_message()
        {
            _caughtException.Message.Should().Be("You have already created a Seleno application; Seleno currently only supports one application at a time per app domain");
        }
    }
}