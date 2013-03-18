using System;
using System.Diagnostics;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Actions
{
    abstract class ElementFinderTests
    {
        public class Finding_an_existant_element : ElementFinderTests
        {
            private Form1Page _page;
            private IWebElement _element;

            public void Given_an_element_exists_on_the_page()
            {
                _page = SelenoHost.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_finding_that_element()
            {
                _element = _page.FindExistantElement;
            }

            public void Then_the_element_was_found()
            {
                Assert.That(_element, Is.Not.Null);
            }
        }

        public class Finding_a_non_existant_element : ElementFinderTests
        {
            private Form1Page _page;
            private Exception _exception;
            private int _maxWait;
            private double _actualWait;

            public void Given_an_element_doesnt_exist_on_the_page()
            {
                _page = SelenoHost.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_finding_that_element_with_a_maximum_wait()
            {
                var stopWatch = Stopwatch.StartNew();
                try
                {
                    _maxWait = 1;
                    #pragma warning disable 168
                    var x = _page.FindNonExistantElement(_maxWait);
                    #pragma warning restore 168
                }
                catch (Exception e)
                {
                    _exception = e;
                }
                _actualWait = stopWatch.Elapsed.TotalSeconds;
            }

            public void Then_an_exception_is_thrown()
            {
                Assert.That(_exception, Is.TypeOf<NoSuchElementException>());
            }

            public void And_the_find_call_waited_the_maximum_amount_of_time()
            {
                Assert.That(_actualWait, Is.GreaterThan(_maxWait - 0.5).And.LessThan(_maxWait + 0.5));
            }
        }

        public class Finding_an_optional_element : ElementFinderTests
        {
            private Form1Page _page;
            private IWebElement _element;

            public void Given_an_element_exists_on_the_page()
            {
                _page = SelenoHost.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_finding_that_element()
            {
                _element = _page.FindOptionalNonExistantElement;
            }

            public void Then_the_element_was_found()
            {
                Assert.That(_element, Is.Null);
            }
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}
