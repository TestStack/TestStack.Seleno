using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FizzWare.NBuilder;
using TestStack.Seleno.Configuration.Contracts;
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
        private readonly IControlIdGenerator _controlIdGenerator = new MvcControlIdGenerator();

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
                .With(m => m.SubViewModel = Builder<TestViewModel>.CreateNew().With(mm => mm.SubViewModel = new TestViewModel{Name = "TripleNestedName"}).Build())
                .Build();
        }

        public void When_inputting_that_model_with_property_type_handling()
        {
            SUT.Model(_model, _propertyHandling);
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

        private void AssertPropertyIgnored<TProperty>(Expression<Func<TestViewModel, TProperty>> property)
        {
            var expectedId = _controlIdGenerator.GetControlId(_controlIdGenerator.GetControlName(property));
            SubstituteFor<IExecutor>().DidNotReceive().Script(Arg.Is<string>(s => s.Contains("#" + expectedId)));
        }

        private void AssertPropertyValueSet<TProperty>(Expression<Func<TestViewModel, TProperty>> property)
        {
            var expectedId = _controlIdGenerator.GetControlId(_controlIdGenerator.GetControlName(property));
            var propertyValue = property.Compile().Invoke(_model);

            var expectedValue = propertyValue.ToString();
            if (propertyValue is DateTime)
                expectedValue = ((DateTime)(object)propertyValue).ToString(DateFormat);

            SubstituteFor<IExecutor>().Received().Script(string.Format("$('#{0}').val(\"{1}\")", expectedId, expectedValue.ToJavaScriptString()));
        }

        public override void Setup()
        {
            SubstituteFor<IComponentFactory>().HtmlControlFor<TextBox>(Arg.Any<LambdaExpression>())
                .Returns(a => new TextBox
                {
                    PageNavigator = SubstituteFor<IPageNavigator>(),
                    Executor = SubstituteFor<IExecutor>(),
                    ControlIdGenerator = _controlIdGenerator
                }
                    .Initialize(a.Arg<LambdaExpression>())
                );
        }
    }
}
