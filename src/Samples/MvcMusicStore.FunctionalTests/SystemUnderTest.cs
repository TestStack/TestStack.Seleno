using System;
using System.IO;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace MvcMusicStore.FunctionalTests
{
    [SetUpFixture]
    public class SystemUnderTest
    {
        [SetUp]
        public void RunIISExpress()
        {
            var dirInfo = new DirectoryInfo(Environment.CurrentDirectory);
            var solutionPath = dirInfo.Parent.Parent.Parent.FullName;
            var path = Path.Combine(solutionPath, "MvcMusicStore");
            IISExpressRunner.Start(path, 12345);
        }     
   
        public static string HomePageAddress
        {
            get
            {
                return IISExpressRunner.HomePage;
            }
        }
    }
}