using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_getting_a_web_element_strongly_typed_text : SpecificationFor<IWebElement>
    {
        private DateTime _result;
        private readonly DateTime _the03rdOfJanuary2012At21h21 = new DateTime(2012, 01, 03, 21, 21, 00);

        public When_getting_a_web_element_strongly_typed_text()
        {

            SUT.Text.Returns("03/01/2012 21:21");
            _result = SUT.TextAsType<DateTime>();
        }

        public void Then_it_should_return_the_requested_strongly_typed_value_of_the_web_element_text()
        {
            _result.Should().Be(_the03rdOfJanuary2012At21h21);
        }
    }
}