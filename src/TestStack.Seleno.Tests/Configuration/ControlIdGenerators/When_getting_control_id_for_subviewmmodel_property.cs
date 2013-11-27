using FluentAssertions;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.ControlIdGenerators
{
    class When_getting_control_id_for_subviewmmodel_property : SpecificationFor<MvcControlIdGenerator>
    {
        private string _generatedId;
        private string _name;

        public void Given_property_is_sub_view_model()
        {
            _name = "SubViewModel.Name";
        }

        public void When_getting_controlid()
        {
            _generatedId = SUT.GetControlId(_name);
        }

        public void Then_correct_id_should_be_generated()
        {
            _generatedId.Should().Be("SubViewModel_Name");
        }
    }
}
