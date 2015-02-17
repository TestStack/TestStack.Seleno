using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_retrieving_textbox_text : HtmlControlSpecificationFor<TextBox, string>
    {
        public When_retrieving_textbox_text() : base(x => x.Name) { }

        public void When_retrieving_the_textbox_text()
        {
            var temp = SUT.Text;
        }

        public void Then_it_should_return_the_correct_content()
        {
            Executor
                .Received()
                .ScriptAndReturn<string>("$('[name=\"Name\"]').val()");
        }
    }
}