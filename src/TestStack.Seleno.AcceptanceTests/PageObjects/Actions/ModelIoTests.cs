using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Actions
{
    abstract class ModelIoTests
    {
        public class Reading_a_model : ModelIoTests
        {
            private Form1Page _page;
            private Form1ViewModel _viewModel;

            public void Given_a_filled_in_form()
            {
                _page = new HomePage().GoToReadModelPage();
            }

            public void When_reading_a_model()
            {
                _viewModel = _page.ReadModel();
            }

            public void Then_the_model_should_match_the_model_used_to_fill_the_form()
            {
                Assert.That(_viewModel, IsSame.ViewModelAs(Form1Fixtures.A));
            }
        }

        public class Writing_a_model : ModelIoTests
        {
            private Form1Page _page;
            private AssertionResultPage _assertion;

            public void Given_an_empty_form()
            {
                _page = new HomePage().GoToWriteModelPage();
            }

            public void When_writing_a_model()
            {
                _assertion = _page.InputFixtureA();
            }

            public void Then_the_model_should_serialise_to_the_same_model()
            {
                _assertion.AssertResult();
            }
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}
