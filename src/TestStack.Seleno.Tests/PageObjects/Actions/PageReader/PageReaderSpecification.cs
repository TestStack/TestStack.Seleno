using System;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    abstract class PageReaderSpecification : ControlSpecification<PageReader<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageReaderSpecification); }
        }
    }
}
