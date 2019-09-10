using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Tests.Specify;
using FluentAssertions;

namespace TestStack.Seleno.Tests.Configuration
{
    abstract class BrowserFactorySpecification : Specification
    {
        public override void EstablishContext()
        {
            Title = "Browser Factory";
        }
    }
}
