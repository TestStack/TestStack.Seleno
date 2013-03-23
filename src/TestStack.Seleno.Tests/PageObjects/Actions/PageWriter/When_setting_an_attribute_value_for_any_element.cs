using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_setting_an_attribute_value_for_any_element : PageWriterSpecification
    {
        private IComponentFactory _componentFactory;
        private IHtmlControl _control;
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";
        private readonly Expression<Func<TestViewModel, string>> _propertySelector = x => x.Name;

        public void Given_the_element_exists()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _control = SubstituteFor<IHtmlControl>();
            
            _componentFactory
                .HtmlControlFor<IHtmlControl>(_propertySelector)
                .Returns(_control);

        }
        
        public void When_setting_an_attribute_value()
        {
            SUT.SetAttribute(_propertySelector, AttributeName, AttributeValue);
        }


        public void Then_the_component_factory_should_retrieve_the_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<IHtmlControl>(_propertySelector);
        }

        public void AndThen_the_control_set_the_attribue_with_the_specified_value()
        {
            _control
                .Received()
                .SetAttributeValue(AttributeName,AttributeValue);
        }
    }
}