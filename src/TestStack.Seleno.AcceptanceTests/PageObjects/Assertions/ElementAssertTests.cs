using NUnit.Framework;
using TestStack.BDDfy;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.PageObjects.Assertions
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

        public class Asserting_an_element_conforms_with_a_rule : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }

            public void When_it_conforms_to_a_certain_rule()
            {}

            public void Then_asserting_element_conformity_passes()
            {
                Page.AssertElementContainsValue("RequiredString", "String");
            }
        }

        public class Asserting_an_element_conforms_with_a_rule_when_it_does_not : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }

            public void When_it_does_not_conform_to_a_certain_rule()
            {}

            public void Then_asserting_element_conformity_throws()
            {
                Assert.That(() => Page.AssertElementContainsValue("RequiredString", "Another Value"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
            }
        }

        public class Asserting_an_element_conforms_with_a_rule_with_jquery : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }

            public void When_it_conforms_to_a_certain_rule()
            {}

            public void Then_asserting_element_conformity_with_jquery_passes()
            {
                Page.AssertElementContainsValueWithJquery("RequiredString", "String");
            }
        }

        public class Asserting_an_element_conforms_with_a_rule_when_it_does_not_with_jquery : ElementAssertTests
        {
            public void Given_an_element_exists_on_the_page()
            {
                NavigateToReadModel();
            }

            public void When_it_does_not_conform_to_a_certain_rule()
            {}

            public void Then_asserting_element_conformity_with_jquery_throws()
            {
                Assert.That(() => Page.AssertElementContainsValueWithJquery("RequiredString", "Another Value"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
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
                Assert.That(() => Page.AssertElementExists("some_non_existing_element"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
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
                Assert.That(() => Page.AssertElementExistsWithJQuery("some_non_existing_element"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
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
                Assert.That(() => Page.AssertElementDoesNotExist("RequiredBool"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
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
                Assert.That(() => Page.AssertElementDoesNotExistWithJquery("RequiredBool"),
                    Throws.InnerException.InstanceOf<SelenoException>()
                );
            }
        }

        [Test]
        public void Perform_test()
        {
            this.BDDfy();
        }
    }
}