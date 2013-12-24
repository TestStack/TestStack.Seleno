namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_inputting_a_model : PageWriterInputSpecification
    {
        public void When_inputting_that_model_with_property_type_handling()
        {
            SUT.Model(Model, PropertyHandling);
        }

        public void Then_input_string_property()
        {
            AssertPropertyValueSet(m => m.Name);
        }

        public void And_input_datetime_property_using_property_type_handling()
        {
            AssertPropertyValueSet(m => m.Modified);
        }

        public void And_ignore_hidden_input_property()
        {
            AssertPropertyIgnored(m => m.HiddenProperty);
        }

        public void And_ignore_readonly_property()
        {
            AssertPropertyIgnored(m => m.ReadonlyProperty);
        }

        public void And_input_non_readonly_property()
        {
            AssertPropertyValueSet(m => m.NonReadonlyProperty);
        }

        public void And_input_scaffolded_property()
        {
            AssertPropertyValueSet(m => m.ScaffoldedProperty);
        }

        public void And_ignore_non_scaffolded_property()
        {
            AssertPropertyIgnored(m => m.NonScaffoldedProperty);
        }

        public void And_input_sub_view_model_string_property()
        {
            AssertPropertyValueSet(m => m.SubViewModel.Name);
        }

        public void And_input_sub_view_model_datetime_property_using_property_type_handling()
        {
            AssertPropertyValueSet(m => m.SubViewModel.Modified);
        }

        public void And_ignore_sub_view_model_hidden_input_property()
        {
            AssertPropertyIgnored(m => m.SubViewModel.HiddenProperty);
        }

        public void And_ignore_sub_view_model_readonly_property()
        {
            AssertPropertyIgnored(m => m.SubViewModel.ReadonlyProperty);
        }

        public void And_input_sub_view_model_non_readonly_property()
        {
            AssertPropertyValueSet(m => m.SubViewModel.NonReadonlyProperty);
        }

        public void And_input_sub_view_model_scaffolded_property()
        {
            AssertPropertyValueSet(m => m.SubViewModel.ScaffoldedProperty);
        }

        public void And_ignore_sub_view_model_non_scaffolded_property()
        {
            AssertPropertyIgnored(m => m.SubViewModel.NonScaffoldedProperty);
        }

        public void And_input_deep_nested_model_property()
        {
            AssertPropertyValueSet(m => m.SubViewModel.SubViewModel.Name);
        }
    }
}
