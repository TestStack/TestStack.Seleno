using System;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Web.PageObjects
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