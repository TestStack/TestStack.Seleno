using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.BDDfy.Scanners.StepScanners.ExecutableAttribute.GwtAttributes;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public class When_getting_a_web_element_strongly_typed_text : PageReaderSpecification
    {
        private DateTime _result;
        private readonly DateTime _the03rdOfJanuary2012At21h21 = new DateTime(2012, 01, 03, 21, 21, 00);

        [Given("Given a web element contains the text 03/01/2012 21:21")]
        public void Given_a_web_element_contains_the_text_03_01_2012_21_21()
        {
            Fake<IElementFinder>()
                .TryFindElement(Arg.Any<By>())
                .Returns(Fake<IWebElement>());

            Fake<IWebElement>().Text.Returns("03/01/2012 21:21");
        }


        public void When_getting_the_web_element_matching_a_view_model_property()
        {
            _result = SUT.TextAsType(viewModel => viewModel.Modified);
        }

        public void Then_it_should_return_the_corresponding_typed_value_of_the_web_element_text()
        {
            _result.Should().Be(_the03rdOfJanuary2012At21h21);
        }
    }
}