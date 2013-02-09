using System;
using System.Web.Mvc;

namespace TestStack.Seleno.Tests.TestObjects
{
    public class TestViewModel
    {
        public string Name { get; set; }
        public DateTime Modified { get; set; }
        public Boolean Exists { get; set; }

        public int Item { get; set; }
        public SelectList Items { get; set; }
    }
}