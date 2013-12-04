using System;
using System.Linq.Expressions;
using FluentAssertions;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.Configuration.ControlIdGenerators
{
    class When_getting_control_name_for_convert_lambda : SpecificationFor<MvcControlIdGenerator>
    {
        private string _generatedName;
        private Expression<Func<TestViewModel, object>> _subViewModelProperty;

        public void Given_property_is_convert_expression()
        {
            _subViewModelProperty = m => m.SubViewModel.Modified;
        }

        public void When_getting_controlname()
        {
            _generatedName = SUT.GetControlName(_subViewModelProperty);
        }

        public void Then_correct_name_should_be_generated()
        {
            _generatedName.Should().Be("SubViewModel.Modified");
        }
    }
}
