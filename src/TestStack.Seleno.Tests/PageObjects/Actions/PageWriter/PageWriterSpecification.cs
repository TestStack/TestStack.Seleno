using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FizzWare.NBuilder;
using NSubstitute;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    abstract class PageWriterSpecification : SpecificationFor<PageWriter<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageWriterSpecification); }
        }
    }

    abstract class PageWriterInputSpecification : PageWriterSpecification
    {
        private const string DateFormat = "d/M/yyyy";
        private readonly IControlIdGenerator _controlIdGenerator = new MvcControlIdGenerator();

        protected TestViewModel Model;
        protected Dictionary<Type, Func<object, string>> PropertyHandling;

        public void Given_a_model()
        {
            Model = Builder<TestViewModel>.CreateNew()
                .With(m => m.SubViewModel = Builder<TestViewModel>.CreateNew().With(mm => mm.SubViewModel = new TestViewModel { Name = "TripleNestedName" }).Build())
                .Build();
        }

        public void AndGiven_property_handling_setup_for_datetime()
        {
            PropertyHandling = new Dictionary<Type, Func<object, string>>
            {
                {typeof(DateTime), d => ((DateTime)d).ToString(DateFormat)}
            };
        }

        protected void AssertPropertyIgnored<TProperty>(Expression<Func<TestViewModel, TProperty>> property)
        {
            var expectedId = _controlIdGenerator.GetControlName(property);
            SubstituteFor<IExecutor>().DidNotReceive().Script(Arg.Is<string>(s => s.Contains("[name=\"" + expectedId)));
        }

        protected void AssertPropertyValueSet<TProperty>(Expression<Func<TestViewModel, TProperty>> property)
        {
            var expectedName = _controlIdGenerator.GetControlName(property);
            var propertyValue = property.Compile().Invoke(Model);

            var expectedValue = propertyValue.ToString();
            if (propertyValue is DateTime)
                expectedValue = ((DateTime)(object)propertyValue).ToString(DateFormat);

            SubstituteFor<IExecutor>().Received().Script(string.Format("$('[name=\"{0}\"]').val(\"{1}\")", expectedName, expectedValue.ToJavaScriptString()));
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