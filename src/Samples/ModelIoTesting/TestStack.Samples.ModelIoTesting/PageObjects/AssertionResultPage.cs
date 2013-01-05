using System;
using TestStack.Seleno.PageObjects;

namespace TestStack.Samples.ModelIoTesting.PageObjects
{
    public class AssertionResultPage : Page
    {
        public void AssertResult()
        {
            var source = Browser.PageSource;
            if (!string.IsNullOrEmpty(source))
                throw new Exception(source);
        }
    }
}