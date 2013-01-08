using System;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    public class PageWriterSpecification : SpecificationFor<PageWriter<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageWriterSpecification); }
        }
    }
}