using System;
using FluentAssertions;
using Funq;
using NSubstitute;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.ViewModels;

using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public class When_populating_view_model_from_the_Page : PageReaderSpecification
    {
        private TestViewModel _viewModel;

        private static readonly DateTime The03rdOfJanuary2012At21h21 = new DateTime(2012, 01, 03, 21, 21, 00);
        private readonly TestViewModel _expectedViewModel = new TestViewModel
                                                                {
                                                                    Exists = true,
                                                                    Modified = The03rdOfJanuary2012At21h21,
                                                                    Name = "someName"
                                                                };

        public void Given_()
        {
            Fake<IScriptExecutor>()
                .ScriptAndReturn(Arg.Any<string>(), Arg.Any<Type>(), Arg.Any<object[]>())
                .Returns(_expectedViewModel.GetPublicPropertyValues());

        }

        public void When_populating_view_model()
        {
            _viewModel = SUT.ModelFromPage();
        }

        public void Then_it_should_execute_script_to_retrieve_value_for_each_matching_element()
        {
            Fake<IScriptExecutor>().Received(3).ScriptAndReturn("$('#{0}').val()", Arg.Any<Type>(), Arg.Any<string>());
        }

        public void AndThen_it_set_the_view_model_property_from_read_values()
        {
            _viewModel.ShouldBeEquivalentTo(_expectedViewModel);
        }
    }
}