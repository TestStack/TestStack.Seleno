using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using TestStack.Seleno.PageObjects.Actions.Fields;
using FluentAssertions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Fields
{
    class ModelFieldValueTests
    {
        #region Single Value Spec
        public class Single_value_ModelFieldValue_spec : TestCaseSpecificationFor<ModelFieldValue>
        {
            private readonly string _testTitle;
            private readonly object _modelValue;
            private readonly bool _expectedIsTrueValue;
            private readonly string _expectedValue;

            public Single_value_ModelFieldValue_spec(string testTitle, object modelValue, bool expectedIsTrueValue, string expectedValue)
            {
                _testTitle = testTitle;
                _modelValue = modelValue;
                _expectedIsTrueValue = expectedIsTrueValue;
                _expectedValue = expectedValue;
            }

            public override string Title
            {
                get { return string.Format("{0} - {1}", _testTitle, _modelValue ?? "(null)"); }
                set { base.Title = value; }
            }

            public void Given_a_ModelFieldValue_of()
            {
                SUT = new ModelFieldValue(_modelValue);
            }

            public void Then_IsTrue_should_be()
            {
                SUT.IsTrue.Should().Be(_expectedIsTrueValue);
            }

            public void And_Value_should_be()
            {
                SUT.Value.Should().Be(_expectedValue);
            }

            public void And_HasMultipleValues_should_be_false()
            {
                SUT.HasMultipleValues.Should().BeFalse();
            }

            public void And_Values_should_throw_exception()
            {
                #pragma warning disable 219
                IEnumerable<string> x;
                #pragma warning restore 219
                Assert.Throws<InvalidOperationException>(() => x = SUT.Values);
            }
        }
        #endregion

        #region Multiple Values Spec
        public class Multiple_values_ModelFieldValue_spec : TestCaseSpecificationFor<ModelFieldValue>
        {
            private readonly string _testTitle;
            private readonly MultipleValuesTestCase _testCase;

            public Multiple_values_ModelFieldValue_spec(string testTitle, MultipleValuesTestCase testCase)
            {
                _testTitle = testTitle;
                _testCase = testCase;
            }

            public override string Title
            {
                get { return string.Format("{0} - {1}", _testTitle, _testCase.ModelValue ?? "(null)"); }
                set { base.Title = value; }
            }

            public void Given_a_ModelFieldValue_of()
            {
                SUT = new ModelFieldValue(_testCase.ModelValue);
            }

            public void Then_IsTrue_should_be_false()
            {
                SUT.IsTrue.Should().BeFalse();
            }

            public void And_Value_should_be()
            {
                SUT.Value.Should().Be(_testCase.ExpectedValue);
            }

            public void And_HasMultipleValues_should_be_true()
            {
                SUT.HasMultipleValues.Should().BeTrue();
            }

            public void And_Values_should_be()
            {
                SUT.Values.Should().Equal(_testCase.ExpectedValues);
            }
        }

        internal class MultipleValuesTestCase
        {
            public object ModelValue { get; set; }
            public string ExpectedValue { get; set; }
            public IEnumerable<string> ExpectedValues { get; set; }
        }
        #endregion
        
        [TestCase(true, true, "true")]
        [TestCase(false, false, "false")]
        [TestCase(null, false, "")]
        public void BooleanTests(bool? modelValue, bool expectedIsTrueValue, string expectedValue)
        {
            new Single_value_ModelFieldValue_spec("Boolean", modelValue, expectedIsTrueValue, expectedValue)
                .Run();
        }

        [TestCase(1, "1")]
        [TestCase(2.3, "2.3")]
        [TestCase(2.333f, "2.333")]
        [TestCase(2.3355551111111, "2.3355551111111")]
        [TestCase((short)5, "5")]
        public void NumericTests(object modelValue, string expectedValue)
        {
            new Single_value_ModelFieldValue_spec("Numeric", modelValue, false, expectedValue)
                .Run();
        }

        [TestCase(null, "")]
        [TestCase(2, "2")]
        public void NullableIntTests(int? modelValue, string expectedValue)
        {
            new Single_value_ModelFieldValue_spec("Nullable Int", modelValue, false, expectedValue)
                .Run();
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("asdf", "asdf")]
        public void StringTests(string modelValue, string expectedValue)
        {
            new Single_value_ModelFieldValue_spec("String", modelValue, false, expectedValue)
                .Run();
        }

        #region Setup Multiple Values
        private static IEnumerable<MultipleValuesTestCase> MultipleValuesTestCases()
        {
            yield return new MultipleValuesTestCase { ModelValue = new object[] {}, ExpectedValue = string.Empty, ExpectedValues = new string[] {}};
            yield return new MultipleValuesTestCase { ModelValue = new [] {"1", "2", "3"}, ExpectedValue = "1,2,3", ExpectedValues = new[] {"1", "2", "3"}};
            yield return new MultipleValuesTestCase { ModelValue = new List<int> { 1, 2, 3 }, ExpectedValue = "1,2,3", ExpectedValues = new[] {"1", "2", "3"}};
            yield return new MultipleValuesTestCase { ModelValue = new Collection<ExampleObject> { new ExampleObject("asdf"), new ExampleObject("1"), new ExampleObject("!@#") }, ExpectedValue = "asdf,1,!@#", ExpectedValues = new string[] {"asdf", "1", "!@#"}};
        }
        internal class ExampleObject
        {
            private readonly string _value;

            public ExampleObject(string value)
            {
                _value = value;
            }

            public override string ToString()
            {
                return _value;
            }
        }
        #endregion

        [TestCaseSource(nameof(MultipleValuesTestCases))]
        public void EnumerableTests(MultipleValuesTestCase testCase)
        {
            new Multiple_values_ModelFieldValue_spec("Enumerable", testCase)
                .Run();
        }
    }
}
