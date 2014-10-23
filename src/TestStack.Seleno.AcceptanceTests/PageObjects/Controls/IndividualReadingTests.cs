using FluentAssertions;
using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.ViewModels;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Controls
{
    [TestFixture]
    public class CheckBoxHtmlControlTests
    {
        [Test]
        public void Reading_checked_checkbox_should_return_true()
        {
            Form1Page page = Host.Instance
                .NavigateToInitialPage<HomePage>()
                .GoToReadModelPage();
            bool result = page.RequiredBoolCheckBoxIsTicked;
            result.Should().BeTrue();
        }
    }
    abstract class Reading_individual_html_controls
    {
        protected void Given_a_filled_in_form()
        {
            Page = Host.Instance
                .NavigateToInitialPage<HomePage>()
                .GoToReadModelPage();
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
        
        public class Reading_textbox_value : Reading_individual_html_controls
        {
            private int _result;

            public void When_getting_the_textbox_value()
            {
                _result = Page.RequiredIntTextBoxValue;
            }

            public void Then_it_should_be_correct()
            {
                _result.Should().Be(1);

            }
        }

        // Moved this into own test (without BDDfy) as was throwing stackoverflow exception.
        // New test is CheckBoxHtmlControlTests.Reading_checked_checkbox_should_return_true (above)
        //public class Reading__checked_checkbox : Reading_individual_html_controls
        //{
        //    private bool _result;

        //    public void When_getting_whether_the_checkbox_is_checked_or_not()
        //    {
        //        _result = Page.RequiredBoolCheckBoxIsTicked;
        //    }

        //    public void Then_the_checkbox_should_be_ticked()
        //    {
        //        _result.Should().BeTrue();

        //    }
        //}

        public class Reading_selected_option_value_from_drop_down : Reading_individual_html_controls
        {
            private SomeEnum _result;

            public void When_getting_selected_option_value()
            {
                _result = Page.RequiredEnumDropDownSelectedValue;
            }

            public void Then_it_should_be_correct()
            {
                _result.Should().Be(SomeEnum.Value2);

            }
        }

        public class Reading_selected_option_text_from_drop_down : Reading_individual_html_controls
        {
            private string _result;

            public void When_getting_selected_option_text()
            {
                _result = Page.RequiredEnumDropDownSelectedText;
            }

            public void Then_it_should_be_correct()
            {
                _result.Should().Be("Value 2");

            }
        }

        public class Reading_selected_radio_button_in_radio_group : Reading_individual_html_controls
        {
            private bool? _result;

            public void When_getting_selected_radio_button_value()
            {
                _result = Page.OptionalBoolAsListSelectedButtonValue;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().Be(null);

            }
        }

        public class Checking_whether_radio_group_has_a_selected_radio_button : Reading_individual_html_controls
        {
            private bool _result;

            public void When_checking_whether_unselected_radio_button_is_selected()
            {
                _result = Page.RequiredListHasSelectedButton;
            }

            public void Then_the_radio_button_shouldnt_be_selected()
            {
                _result.Should().BeFalse();
            }
        }

        public class Reading_multiline_textarea_content : Reading_individual_html_controls
        {
            private string _actualMultiLinesContent;

            public void When_getting_the_textarea_content()
            {
                _actualMultiLinesContent = Page.TextAreaFieldContent;
            }

            public void Then_the_textArea_multiLine_content_value_should_match()
            {
                _actualMultiLinesContent.Should().Be(Form1Fixtures.A.TextAreaField);
            }
        }
        
        protected Form1Page Page;
    }    
}