using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.Web.Fixtures;
using TestStack.Seleno.AcceptanceTests.Web.PageObjects;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Actions
{
    abstract class ElementFinderTests
    {
        public class Finding_an_existant_element : ElementFinderTests
        {
            protected Form1Page Page;
            private IWebElement _element;

            public void Given_an_element_exists_on_the_page()
            {
                Page = Host.Instance.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_finding_that_element()
            {
                _element = PerformFind();
            }

            protected virtual IWebElement PerformFind()
            {
                return Page.FindExistentElement;
            }

            public void Then_the_element_was_found()
            {
                Assert.That(_element, Is.Not.Null);
            }
        }

        public class Finding_an_existant_element_by_jquery : Finding_an_existant_element
        {
            protected override IWebElement PerformFind()
            {
                return Page.FindExistentElementByJQuery;
            }
        }

        public class Finding_a_non_existant_element : ElementFinderTests
        {
            protected Form1Page Page;
            private Exception _exception;
            private int _maxWait;
            private double _actualWait;

            public void Given_an_element_doesnt_exist_on_the_page()
            {
                Page = Host.Instance.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_finding_that_element_with_a_maximum_wait()
            {
                var stopWatch = Stopwatch.StartNew();
                try
                {
                    _maxWait = 3;
                    #pragma warning disable 168
                    var x = PerformFind(_maxWait);
                    #pragma warning restore 168
                }
                catch (Exception e)
                {
                    _exception = e;
                }
                _actualWait = stopWatch.Elapsed.TotalSeconds;
            }

            protected virtual IWebElement PerformFind(int secondsToWait)
            {
                return Page.FindNonExistentElement(secondsToWait);
            }

            public void Then_an_exception_is_thrown()
            {
                Assert.That(_exception, Is.TypeOf<NoSuchElementException>());
            }

            public void And_the_find_call_waited_the_maximum_amount_of_time()
            {
                Assert.That(_actualWait, Is.EqualTo(_maxWait).Within(2));
            }
        }

        public class Finding_a_non_existant_element_by_jquery : Finding_a_non_existant_element
        {
            protected override IWebElement PerformFind(int secondsToWait)
            {
                return Page.FindNonExistentElementByJQuery(secondsToWait);
            }
        }

        public class Finding_an_optional_element : ElementFinderTests
        {
            protected Form1Page Page;
            private IWebElement _element;

            public void Given_an_element_doesnt_exist_on_the_page()
            {
                Page = Host.Instance.NavigateToInitialPage<HomePage>()
                    .GoToReadModelPage();
            }

            public void When_optionally_finding_that_element()
            {
                _element = PerformFind();
            }

            protected virtual IWebElement PerformFind()
            {
                return Page.FindOptionalNonExistentElement;
            }

            public void Then_the_element_is_null()
            {
                Assert.That(_element, Is.Null);
            }
        }

        public class Finding_an_optional_element_by_jquery : Finding_an_optional_element
        {
            protected override IWebElement PerformFind()
            {
                return Page.FindOptionalNonExistentElementByJQuery;
            }
        }

        public class Finding_existant_elements : ElementFinderTests
        {
            protected ListPage Page;
            private IEnumerable<IWebElement> _elements;

            public void Given_elements_exist_on_the_page()
            {
                Page = Host.Instance.NavigateToInitialPage<HomePage>()
                    .GoToListPage();
            }

            public void When_finding_those_elements()
            {
                _elements = PerformFind();
            }

            protected virtual IEnumerable<IWebElement> PerformFind()
            {
                return Page.Items;
            }

            public void Then_the_elements_were_found()
            {
                Assert.That(_elements, Is.Not.Null);
            }

            public void And_all_the_elements_on_the_page_were_returned()
            {
                Assert.That(_elements.Select(e => e.Text).ToArray(), Is.EqualTo(ListFixtures.ListItems));
            }
        }

        public class Finding_existant_elements_by_jquery : Finding_existant_elements
        {
            protected override IEnumerable<IWebElement> PerformFind()
            {
                return Page.ItemsByJQuery;
            }
        }

        public class Finding_nonexistant_elements : ElementFinderTests
        {
            protected ListPage Page;
            private IEnumerable<IWebElement> _elements;
            private int _maxWait;
            private double _elapsedTime;

            public void Given_no_elements_exist_on_the_page_that_match_the_search()
            {
                Page = Host.Instance.NavigateToInitialPage<HomePage>()
                    .GoToListPage();
            }

            public void And_given_the_max_wait_is_3_seconds()
            {
                _maxWait = 3;
            }

            public void When_finding_those_elements()
            {
                var stopwatch = Stopwatch.StartNew();
                _elements = PerformFind(_maxWait);
                stopwatch.Stop();
                _elapsedTime = stopwatch.Elapsed.TotalSeconds;
            }

            protected virtual IEnumerable<IWebElement> PerformFind(int secondsToWait)
            {
                return Page.FindNonExistantItems(TimeSpan.FromSeconds(secondsToWait));
            }

            public void Then_no_elements_were_found()
            {
                Assert.That(_elements, Is.Empty);
            }

            public void And_the_find_called_waited_approximately_3_seconds()
            {
                Assert.That(_elapsedTime, Is.EqualTo(_maxWait).Within(2));
            }
        }

        public class Finding_nonexistant_elements_by_jquery : Finding_nonexistant_elements
        {
            protected override IEnumerable<IWebElement> PerformFind(int secondsToWait)
            {
                return Page.FindNonExistantItemsByJQuery(TimeSpan.FromSeconds(secondsToWait));
            }
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}
