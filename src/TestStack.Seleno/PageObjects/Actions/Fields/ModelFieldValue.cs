using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TestStack.Seleno.PageObjects.Actions.Fields
{
    internal interface IModelFieldValue
    {
        bool HasMultipleValues { get; }
        IEnumerable<string> Values { get; }
        string Value { get; }
        bool IsTrue { get; }
    }

    internal class ModelFieldValue : IModelFieldValue
    {
        private readonly object _value;

        public ModelFieldValue(object value)
        {
            _value = value;
            var test = 2.3d;
        }

        public bool HasMultipleValues => _value as IEnumerable != null && _value.GetType() != typeof(string);

        public IEnumerable<string> Values
        {
            get
            {
                if (!HasMultipleValues)
                    throw new InvalidOperationException("Field does not have multiple values!");
                return (_value as IEnumerable).Cast<object>()
                    .Select(o => new ModelFieldValue(o))
                    .Select(v => v.Value);
            }
        }

        private string ValueToStringWithInvariantCulture()
        {
            var toStringMethod = _value.GetType().GetMethod("ToString",
                BindingFlags.Public | BindingFlags.Instance,
                null, new[] {typeof (IFormatProvider)}, null);
            if (toStringMethod != null)
                return (string)toStringMethod.Invoke(_value, new object[] { CultureInfo.InvariantCulture });
            return _value.ToString();
        }

        public string Value
        {
            get
            {
                if (HasMultipleValues)
                    return string.Join(",", Values);
                if (_value is bool)
                    return _value.ToString().ToLower();
                return _value != null ? ValueToStringWithInvariantCulture() : string.Empty;
            }
        }

        public bool IsTrue => _value as bool? == true;
    }
}
