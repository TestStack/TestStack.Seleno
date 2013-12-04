using System;
using System.Linq.Expressions;
using FluentAssertions;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.Configuration.ControlIdGenerators
{
    class When_getting_control_name_for_subviewmmodel_property : SpecificationFor<MvcControlIdGenerator>
    {
        private string _generatedName;
        private Expression<Func<TestViewModel, string>> _subViewModelProperty;

        public void Given_property_is_sub_view_model()
        {
            _subViewModelProperty = m => m.SubViewModel.Name;
        }

        public void When_getting_controlname()
        {
            _generatedName = SUT.GetControlName(_subViewModelProperty);
        }

        public void Then_correct_name_should_be_generated()
        {
            _generatedName.Should().Be("SubViewModel.Name");
        }
    }
}
