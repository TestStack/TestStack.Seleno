namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_inputting_a_simple_field : PageWriterInputSpecification
    {
        public void When_inputting_a_simple_field_with_property_type_handling()
        {
            SUT.Field(z => z.Modified, Model.Modified, PropertyHandling);
        }

        public void Then_input_field_using_property_type_handling()
        {
            AssertPropertyValueSet(m => m.Modified);
        }
    }

    class When_inputting_a_complex_field : PageWriterInputSpecification
    {
        public void When_inputting_a_complex_field_value()
        {
            SUT.Field(z => z.SubViewModel, Model.SubViewModel);
        }

        public void Then_input_nested_field()
        {
            AssertPropertyValueSet(m => m.SubViewModel.Name);
        }

        public void And_input_deep_nested_field()
        {
            AssertPropertyValueSet(m => m.SubViewModel.SubViewModel.Name);
        }

        public void And_ignore_subling_properties()
        {
            AssertPropertyIgnored(m => m.Modified);
        }
    }
}
