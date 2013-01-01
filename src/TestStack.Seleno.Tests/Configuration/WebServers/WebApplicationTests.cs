using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.Tests.Configuration.WebServers
{
    [TestFixture]
    public class WebApplicationTests
    {
        [Test]
        public void WebApplication_should_have_PortNumber()
        {
            Action action = () =>  new WebApplication(Substitute.For<IProjectLocation>(), 0);
            action.ShouldThrow<ArgumentException>()
                .WithMessage("portNumber must be greater than zero");
        }

        [Test]
        public void WebApplication_should_have_Location()
        {
            Action action = () =>  new WebApplication(null, 1000);
            action.ShouldThrow<ArgumentException>()
                .WithMessage("You must specify a location");
        }

    }
}
