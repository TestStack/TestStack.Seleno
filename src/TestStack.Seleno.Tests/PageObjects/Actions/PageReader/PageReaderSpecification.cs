using System;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public abstract class PageReaderSpecification : SpecificationFor<PageReader<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageReaderSpecification); }
        }
    }
}
