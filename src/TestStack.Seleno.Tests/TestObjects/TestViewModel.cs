using System;

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
    }

    public enum ChoiceType
    {
        None,
        Other,
        Another
    }
}