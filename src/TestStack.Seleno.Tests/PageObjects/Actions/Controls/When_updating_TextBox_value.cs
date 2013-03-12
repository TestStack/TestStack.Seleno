using System;
using System.Globalization;
using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_updating_textbox_value_with_date : HtmlControlSpecificationFor<TextBox>
    {
        private static readonly DateTime _the03rdOfJanuary2012At21h21 = new DateTime(2012, 01, 03, 21, 21, 00);
        private readonly string _expectedScriptToBeExecuted =
            string.Format(@"$('#Modified').val(""{0}"")",_the03rdOfJanuary2012At21h21.ToString(CultureInfo.CurrentCulture));

        public When_updating_textbox_value_with_date() : base(x => x.Modified) { }

        public void When_updating_the_textbox_value()
        {
            SUT.ReplaceInputValueWith(_the03rdOfJanuary2012At21h21);
        }

        public void Then_script_executor_should_execute_relevant_script_to_replace_the_value()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript(_expectedScriptToBeExecuted);
        }
    }

    class When_updating_textbox_value_with_string : HtmlControlSpecificationFor<TextBox>
    {
        public When_updating_textbox_value_with_string() : base(x => x.Modified) { }

        public void When_updating_the_textbox_value()
        {
            SUT.ReplaceInputValueWith("asdf \\ \" \r\n");
        }

        public void Then_script_executor_should_execute_relevant_script_to_replace_the_value()
        {
            ScriptExecutor
                .Received()
                .ExecuteScript(@"$('#Modified').val(""asdf \\ \"" \r\n"")");
        }
    }
}
