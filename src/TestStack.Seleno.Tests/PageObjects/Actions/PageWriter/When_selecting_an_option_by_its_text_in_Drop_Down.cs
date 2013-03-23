using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_selecting_an_option_by_its_text_in_drop_down : PageWriterSpecification
    {
        private IComponentFactory _componentFactory;
        private DropDown _dropDown;
        private readonly Expression<Func<TestViewModel, int>> _dropDownPropertySelector = x => x.Item;

        public void Given_a_drop_down_exists()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _dropDown = Substitute.For<DropDown>();

            _componentFactory
                .HtmlControlFor<DropDown>(_dropDownPropertySelector)
                .Returns(_dropDown);
            
        }
        
        public void When_selecting_an_option_by_its_value()
        {
            SUT.SelectByOptionTextInDropDown(_dropDownPropertySelector, "other...");
        }

        public void Then_the_component_factory_should_retrieve_the_drop_down_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<DropDown>(_dropDownPropertySelector);
        }

        public void AndThen_the_drop_down_selected_the_option_matching_the_specified_text()
        {
            _dropDown.Received().SelectElementByText("other...");
        }
    }
}