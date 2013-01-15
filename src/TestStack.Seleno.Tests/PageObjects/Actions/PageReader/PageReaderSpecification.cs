using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Humanizer;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.ViewModels;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public class PageReaderSpecification : SpecificationFor<PageReader<TestViewModel>>
    {
        public override Type Story
        {
            get { return typeof (PageReaderSpecification); }
        }
    }
}
