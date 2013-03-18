using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_clearing_and_sending_keys_to_web_element : PageWriterSpecification
    {
        private const string SentText = "Some text";

        public void Given_there_is_a_web_element_matching_By_Name()
        {
            SubstituteFor<IElementFinder>()
                .Element(By.Name("Name"))
                .Returns(SubstituteFor<IWebElement>());
        }

        public void when_clearing_and_sending_keys()
        {
            SUT.ClearAndSendKeys(x => x.Name,SentText);
        }

        public void Then_it_should_clear_any_existing_text_within_web_element()
        {
            SubstituteFor<IWebElement>().Received(1).Clear();
        }

        public void And_it_should_send_the_new_text_to_the_web_element()
        {
            SubstituteFor<IWebElement>().Received(1).SendKeys(SentText);
        }
    }
}
