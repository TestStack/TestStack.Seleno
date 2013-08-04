using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Controls
{
    abstract class Writing_individual_html_controls
    {
        public class Writing_a_value_to_textbox : Writing_individual_html_controls
        {

            public void When_writing_a_value_to_a_textbox()
            {
                Page.RequiredIntTextBoxValue = 125;
            }

            public void Then_the_textbox_value_should_be_set()
            {
                Page.RequiredIntTextBoxValue.Should().Be(125);
            }
        }

        public class Ticking_a_checkBox : Writing_individual_html_controls
        {
            public void When_ticking_a_checkbox()
            {
                Page.RequiredBoolCheckBoxIsTicked = true;
            }

            public void Then_the_checkbox_should_be_ticked()
            {
                Page.RequiredBoolCheckBoxIsTicked.Should().BeTrue();

            }
        }

        public class Selecting_option_by_its_value_in_a_Drop_Down : Writing_individual_html_controls
        {
            public void When_selecting_an_option_by_its_value()
            {
                Page.RequiredEnumDropDownSelectedValue = SomeEnum.Value3;
            }

            public void Then_the_option_should_be_selected()
            {
                Page.RequiredEnumDropDownSelectedValue.Should().Be(SomeEnum.Value3);

            }
        }

        public class Selecting_option_by_its_text_in_a_drop_down : Writing_individual_html_controls
        {
            public void When_selecting_an_option_by_its_text()
            {
                Page.RequiredEnumDropDownSelectedText = "Value 2";
            }

            public void Then_the_option_should_be_selected()
            {
                Page.RequiredEnumDropDownSelectedText.Should().Be("Value 2");

            }
        }

        public class Selecting_radio_button_in_radio_group : Writing_individual_html_controls
        {
            public void When_selecting_a_radio_button()
            {
                Page.OptionalBoolAsListSelectedButtonValue = false;
            }

            public void Then_the_radio_should_be_selected()
            {
                Page.OptionalBoolAsListSelectedButtonValue.Should().Be(false);
            }
        }

        public class Writing_textarea_content : Writing_individual_html_controls
        {
            public void When_writing_the_textArea_content()
            {
                Page.TextAreaFieldContent = Form1Fixtures.A.TextAreaField;
            }

            public void Then_the_textArea_multiLine_content_value_should_match()
            {
                Page.TextAreaFieldContent.Should().Be(Form1Fixtures.A.TextAreaField);
            }
        }

        protected Form1Page Page;

        protected void Given_a_filled_in_form()
        {
            Page = Host.Instance
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