using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.Specifications.Assertions
{
    public abstract class ElementAssertTests
    {
        protected Form1Page Page;

        private void NavigateToReadModel()
        {
            Page = Host.Instance.NavigateToInitialPage<HomePage>()
                .GoToReadModelPage();
        }

        public class Asserting_an_element_exists_when_it_exists : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }

            public void Then_asserting_element_existence_passes()
            {
                Page.AssertElementExists("RequiredBool");
            }
        }

        public class Asserting_an_element_exists_when_it_doesnt_exist : ElementAssertTests
        {
            public void Given_an_element_does_not_exist_on_the_page()
            {
                NavigateToReadModel();
            }

            public void Then_asserting_element_existence_throws()
            {
                Assert.Throws<SelenoException>(() =>
                    Page.AssertElementExists("some_non_existing_element"));
            }
        }

        public class Asserting_an_element_exists_when_it_doesnt_exist_with_jquery : ElementAssertTests
        {
            public void Given_an_element_does_not_exist_on_the_page()
            {
                NavigateToReadModel();
            }

            public void Then_asserting_element_existence_throws()
            {
                Assert.Throws<SelenoException>(() =>
                    Page.AssertElementExistsWithJQuery("some_non_existing_element"));
            }
        }

        public class Asserting_an_element_exists_with_jQuery : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }
            
            public void Then_asserting_element_existence_passes()
            {
                Page.AssertElementExistsWithJQuery("RequiredBool");
            }
        }

        public class Asserting_an_element_does_not_exist : ElementAssertTests
        {
            public void Given_an_element_does_not_exist_on_the_page()
            {
                NavigateToReadModel();
            }
            
            public void Then_asserting_element_does_not_exist_passes()
            {
                Page.AssertElementDoesNotExist("non_existing_element");
            }
        }

        public class Asserting_an_element_does_not_exist_when_it_exists : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }
            
            public void Then_asserting_element_does_not_exist_throws()
            {
                Assert.Throws<SelenoException>(() => Page.AssertElementDoesNotExist("RequiredBool"));
            }
        }

        public class Asserting_an_element_does_not_exist_with_jquery : ElementAssertTests
        {
            public void Given_an_element_does_not_exist_on_the_page()
            {
                NavigateToReadModel();
            }
            
            public void Then_asserting_element_does_not_exist_passes()
            {
                Page.AssertElementDoesNotExistWithJquery("non_existing_element");
            }
        }

        public class Asserting_an_element_does_not_exist_when_it_exists_with_jquery : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }
            
            public void Then_asserting_element_does_not_exist_throws()
            {
                Assert.Throws<SelenoException>(() => Page.AssertElementDoesNotExistWithJquery("RequiredBool"));
            }
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}