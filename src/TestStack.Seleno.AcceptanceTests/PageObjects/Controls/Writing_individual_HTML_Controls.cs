using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Controls
{
    abstract class Writing_individual_HTML_Controls
    {

        public class Writing_a_value_in_textbox : Writing_individual_HTML_Controls
        {

            public void When_writing_a_value_to_the_textbox()
            {
                _page.RequiredIntTextBoxValue = 125;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _page.RequiredIntTextBoxValue.Should().Be(125);
            }
        }

        public class Ticking_a_checkBox : Writing_individual_HTML_Controls
        {
            public void When_ticking_the_checkbox()
            {
                _page.RequiredBoolCheckBoxIsTicked = true;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _page.RequiredBoolCheckBoxIsTicked.Should().BeTrue();

            }
        }

        public class Selecting_option_by_its_value_in_Drop_Down : Writing_individual_HTML_Controls
        {
            public void When_selecting_option_by_its_value()
            {
                _page.RequiredEnumDropDownSelectedValue = SomeEnum.Value3;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _page.RequiredEnumDropDownSelectedValue.Should().Be(SomeEnum.Value3);

            }
        }

        public class Selecting_option_by_its_text_in_Drop_Down : Writing_individual_HTML_Controls
        {

            public void When_selecting_option_by_its_text()
            {
                _page.RequiredEnumDropDownSelectedText = "Value 2";
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _page.RequiredEnumDropDownSelectedText.Should().Be("Value 2");

            }
        }

        public class Selecting_radio_button_in_radio_group : Writing_individual_HTML_Controls
        {

            public void When_selecting_a_radio_button()
            {
                _page.OptionalBoolSelectedButtonValue = false;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _page.OptionalBoolSelectedButtonValue.Should().Be(false);
            }
        }
        
        public class Writing_multiLine_textArea_content : Writing_individual_HTML_Controls
        {
            private readonly string[] _multiLinesContent =
                Form1Fixtures.A.TextAreaField.Split('\r')
                             .Select(line => line.Replace("\n", string.Empty)).ToArray();

            public void When_writing_the_textArea_content()
            {
                _page.TextAreaFieldContent = _multiLinesContent;
            }

            public void Then_the_textArea_multiLine_content_value_should_match()
            {
                _page.TextAreaFieldContent.Should().HaveCount(4).And.ContainInOrder(_multiLinesContent);
            }
        }


        protected Form1Page _page;

        protected void Given_a_filled_in_form()
        {
            _page =
                SelenoHost
                    .NavigateToInitialPage<HomePage>()
                    .GoToWriteModelPage();
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}