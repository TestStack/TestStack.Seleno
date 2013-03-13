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
    abstract class Reading_individual_HTML_Controls
    {
        public class Reading_textox_value : Reading_individual_HTML_Controls
        {
            private int _result;

            public void When_getting_the_textbox_value()
            {
                _result = _page.RequiredIntTextBoxValue;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().Be(1);

            }
        }
        
        public class Reading_checkBox_is_ticked : Reading_individual_HTML_Controls
        {
            private bool _result;

            public void When_getting_whether_the_checkbox_is_checked_or_not()
            {
                _result = _page.RequiredBoolCheckBoxIsTicked;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().BeTrue();

            }
        }

        public class Reading_selected_option_value_in_Drop_Down  : Reading_individual_HTML_Controls
        {
            private SomeEnum _result;

            public void When_getting_selected_option_value()
            {
                _result = _page.RequiredEnumDropDownSelectedValue;
            }
            
            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().Be(SomeEnum.Value2);

            }
        }

        public class Reading_selected_option_text_in_Drop_Down : Reading_individual_HTML_Controls
        {
            private string _result;

            public void When_getting_selected_option_value()
            {
                _result = _page.RequiredEnumDropDownSelectedText;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().Be("Value 2");

            }
        }

        public class Reading_selected_radio_button_in_radio_group : Reading_individual_HTML_Controls
        {
            private bool? _result;

            public void When_getting_selected_radio_button_value()
            {
                _result = _page.OptionalBoolSelectedButtonValue;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().BeTrue();

            }
        }

        public class Checking_whether_radio_group_has_a_selected_radio_button : Reading_individual_HTML_Controls
        {
            private bool _result;

            public void When_checking_whether_radio_button_is_selected()
            {
                _result = _page.OptionalListHasSelectedButton;
            }

            public void Then_the_checkBox_value_should_be_ticked()
            {
                _result.Should().BeFalse();

            }
        }
      
        public class Reading_multiLine_textArea_content : Reading_individual_HTML_Controls
        {
            private string[] _actualMultiLinesContent;

            private readonly string[] _expectedMultiLinesContent =
                Form1Fixtures.A.TextAreaField.Split('\r')
                    .Select(line => line.Replace("\n", string.Empty))
                    .ToArray();

            public void When_getting_the_textArea_content()
            {
                _actualMultiLinesContent = _page.TextAreaFieldContent;
            }

            public void Then_the_textArea_multiLine_content_value_should_match()
            {
                _actualMultiLinesContent.Should().HaveCount(4).And.ContainInOrder(_expectedMultiLinesContent);
            }
        }
        
        protected Form1Page _page;

        protected void Given_a_filled_in_form()
        {
            _page =
                SelenoHost
                    .NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
    
}