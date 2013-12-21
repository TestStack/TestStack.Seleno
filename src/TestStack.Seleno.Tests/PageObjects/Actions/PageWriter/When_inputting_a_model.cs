using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;
using NSubstitute;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_inputting_a_model : PageWriterSpecification
    {
        private const string DateFormat = "d/M/yyyy";
        private TestViewModel _model;
        private Dictionary<Type, Func<object, string>> _propertyHandling;

        public void AndGiven_property_handling_setup_for_datetime()
        {
            _propertyHandling = new Dictionary<Type, Func<object, string>>
            {
                {typeof(DateTime), d => ((DateTime)d).ToString(DateFormat)}
            };
        }

        public void Given_a_model()
        {
            _model = Builder<TestViewModel>.CreateNew()
                .With(m => m.SubViewModel = Builder<TestViewModel>.CreateNew().Build())
                .Build();
        }

        public void When_inputting_that_model_with_property_type_handling()
        {
            SUT.Model(_model, _propertyHandling);
        }

        public void Then_input_string_property()
        {
            AssertPropertyValueSet("Name", _model.Name);
        }

        public void And_input_datetime_property_using_property_type_handling()
        {
            AssertPropertyValueSet("Modified", _model.Modified.ToString(DateFormat));
        }

        public void And_ignore_hidden_input_property()
        {
            AssertPropertyIgnored("HiddenProperty");
        }

        public void And_ignore_readonly_property()
        {
            AssertPropertyIgnored("ReadonlyProperty");
        }

        public void And_input_non_readonly_property()
        {
            AssertPropertyValueSet("NonReadonlyProperty", _model.NonReadonlyProperty);
        }

        public void And_input_scaffolded_property()
        {
            AssertPropertyValueSet("ScaffoldedProperty", _model.ScaffoldedProperty);
        }

        public void And_ignore_non_scaffolded_property()
        {
            AssertPropertyIgnored("NonScaffoldedProperty");
        }

        public void And_input_sub_view_model_string_property()
        {
            AssertPropertyValueSet("SubViewModel_Name", _model.SubViewModel.Name);
        }

        public void And_input_sub_view_model_datetime_property_using_property_type_handling()
        {
            AssertPropertyValueSet("SubViewModel_Modified", _model.Modified.ToString(DateFormat));
        }

        public void And_ignore_sub_view_model_hidden_input_property()
        {
            AssertPropertyIgnored("SubViewModel_HiddenProperty");
        }

        public void And_ignore_sub_view_model_readonly_property()
        {
            AssertPropertyIgnored("SubViewModel_ReadonlyProperty");
        }

        public void And_input_sub_view_model_non_readonly_property()
        {
            AssertPropertyValueSet("SubViewModel_NonReadonlyProperty", _model.SubViewModel.NonReadonlyProperty);
        }

        public void And_input_sub_view_model_scaffolded_property()
        {
            AssertPropertyValueSet("SubViewModel_ScaffoldedProperty", _model.SubViewModel.ScaffoldedProperty);
        }

        public void And_ignore_sub_view_model_non_scaffolded_property()
        {
            AssertPropertyIgnored("SubViewModel_NonScaffoldedProperty");
        }
        
        private void AssertPropertyIgnored(string fieldId)
        {
            SubstituteFor<IExecutor>().DidNotReceive().Script(Arg.Is<string>(s => s.Contains("#" + fieldId)));
        }

        private void AssertPropertyValueSet(string fieldId, string fieldValue)
        {
            SubstituteFor<IExecutor>().Received().Script(string.Format("$('#{0}').val(\"{1}\")", fieldId, fieldValue.ToJavaScriptString()));
        }

        public override void Setup()
        {
            SubstituteFor<IComponentFactory>().HtmlControlFor<TextBox>(Arg.Any<string>())
                .Returns(a => new TextBox
                {
                    PageNavigator = SubstituteFor<IPageNavigator>(),
                    Executor = SubstituteFor<IExecutor>(),
                    ControlIdGenerator = new MvcControlIdGenerator()
                }
                    .Initialize(a.Arg<string>())
                );
        }
    }
}
