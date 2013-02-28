using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Controls
{
    class Reading_from_Form_HTML_Controls 
    {

        private Form1Page _page;
        private bool _result;

        public void Given_a_filled_in_form()
            {
                _page = SelenoHost.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

        public void When_getting_whether_the_checkbox_is_checked_or_not()
        {
            _result = _page.RequiredBoolCheckBoxIsTicked;
        }

        public void Then_the_checkBox_value_should_be_ticked()
        {
            _result.Should().BeTrue();

        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}