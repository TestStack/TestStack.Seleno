using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    [TestFixture]
    public class When_only_sending_keys_to_web_element
    {
        private readonly IWebElement _webElement = Substitute.For<IWebElement>();
        private const string SentText = "Some text";

        public When_only_sending_keys_to_web_element()
        {
            _webElement.ClearAndSendKeys(SentText,false);
        }

        [Test]
        public void Then_it_should_not_clear_any_existing_text_within_web_element()
        {
            _webElement.DidNotReceive().Clear();
        }

        [Test]
        public void And_it_should_send_the_new_text_to_the_web_element()
        {
            _webElement.Received(1).SendKeys(SentText);
        }
    }
}