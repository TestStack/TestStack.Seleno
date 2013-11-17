using System;
using System.Globalization;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    abstract class TextboxControlSpecification<TValueType> : HtmlControlSpecificationFor<TextBox, TValueType>
    {
        protected TextboxControlSpecification(Expression<Func<TestViewModel, TValueType>> htmlControlPropertySelector)
            : base(htmlControlPropertySelector) {}
        protected abstract TValueType Value { get; }
        protected abstract string ExpectedControlId { get; }
        protected abstract string ExpectedControlValue { get; }

        public void When_updating_the_textbox_value()
        {
            SUT.ReplaceInputValueWith(Value);
        }

        public void Then_script_executor_should_execute_relevant_script_to_replace_the_value()
        {
            Executor
                .Received()
                .Script(string.Format("$('#{0}').val(\"{1}\")", ExpectedControlId, ExpectedControlValue));
        }
    }

    class When_updating_textbox_value_with_date : TextboxControlSpecification<DateTime>
    {
        private static readonly DateTime ThirdOfJanuary2012AtNineTwentyOnePm = new DateTime(2012, 01, 03, 21, 21, 00);

        public When_updating_textbox_value_with_date() : base(x => x.Modified) { }

        protected override DateTime Value
        {
            get { return ThirdOfJanuary2012AtNineTwentyOnePm; }
        }

        protected override string ExpectedControlId
        {
            get { return "Modified"; }
        }

        protected override string ExpectedControlValue
        {
            get { return ThirdOfJanuary2012AtNineTwentyOnePm.ToString(CultureInfo.CurrentCulture); }
        }
    }

    class When_updating_textbox_value_with_string : TextboxControlSpecification<string>
    {
        public When_updating_textbox_value_with_string() : base(x => x.Name) { }

        protected override string Value
        {
            get { return "asdf \\ \" \r\n"; }
        }

        protected override string ExpectedControlId
        {
            get { return "Name"; }
        }

        protected override string ExpectedControlValue
        {
            get { return @"asdf \\ \"" \r\n"; }
        }
    }

    class When_updating_subviewmodel_textbox_value_with_string : TextboxControlSpecification<string>
    {
        public When_updating_subviewmodel_textbox_value_with_string() : base(x => x.SubViewModel.Name) { }

        protected override string Value
        {
            get { return "asdf \\ \" \r\n"; }
        }

        protected override string ExpectedControlId
        {
            get { return "SubViewModel_Name"; }
        }

        protected override string ExpectedControlValue
        {
            get { return @"asdf \\ \"" \r\n"; }
        }
    }
}
