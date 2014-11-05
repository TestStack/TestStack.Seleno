using System;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_taking_screenshot : SelenoApplicationSpecification
    {
        private string _imageName = "screenshot";
        private string _errorMessage = "there was an error";
        private string _fileName;
        private Exception _result;

        public override void EstablishContext()
        {
            var dateTime = new DateTime(2014, 05, 11, 10, 29, 33);
            _fileName = string.Format(@"{0}{1}.png", _imageName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));
            SystemTime.Now = () => dateTime;
        }

        public void Given_initialised_application()
        {
            SUT.Initialize();
        }

        public void When_taking_screenshot_and_throwing()
        {
            _result = Catch.Exception(() => SUT.TakeScreenshotAndThrow(_imageName, _errorMessage));
        }

        public void Then_should_take_screenshot()
        {
            SubstituteFor<ICamera>().Received().TakeScreenshot(_fileName);
        }

        public void AndThen_should_throw_SelenoException()
        {
            _result.Should().BeOfType<SelenoException>()
                .Which.Message.Should().Be(_errorMessage);
        }

        public override void TearDown()
        {
            SystemTime.Reset();
        }
    }
}
