using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestStack.Seleno.Tests.TestObjects
{
    public class TestViewModel
    {
        public string Name { get; set; }
        public DateTime Modified { get; set; }
        public Boolean Exists { get; set; }

        public int Item { get; set; }
        public ChoiceType Choice { get; set; }
        public string MultiLineContent { get; set; }
        public string AnotherChoice { get; set; }

        public TestViewModel SubViewModel { get; set; }

        [HiddenInput]
        public string HiddenProperty { get; set; }
        [ReadOnly(true)]
        public string ReadonlyProperty { get; set; }
        [ReadOnly(false)]
        public string NonReadonlyProperty { get; set; }
        [ScaffoldColumn(true)]
        public string ScaffoldedProperty { get; set; }
        [ScaffoldColumn(false)]
        public string NonScaffoldedProperty { get; set; }
    }

    public enum ChoiceType
    {
        None,
        Other,
        Another
    }
}