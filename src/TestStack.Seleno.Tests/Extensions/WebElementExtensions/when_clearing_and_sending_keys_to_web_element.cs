using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class when_clearing_and_sending_keys_to_web_element : SpecificationFor<IWebElement>
    {
        private const string SentText = "Some text";

        public when_clearing_and_sending_keys_to_web_element()
        {
            SUT.ClearAndSendKeys(SentText);
        }

        public void Then_it_should_clear_any_existing_text_within_web_element()
        {
            SUT.Received(1).Clear();
        }

        public void And_it_should_send_the_new_text_to_the_web_element()
        {
            SUT.Received(1).SendKeys(SentText);
        }
    }
}
