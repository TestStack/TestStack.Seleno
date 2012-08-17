using System;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step4.Pages;

namespace TestStack.Seleno.Samples.MvcMusicStore.FunctionalTests.Step4
{
    public class Application : Page
    {
        public Application()
        {
            SelenoApplicationRunner.Run("TestStack.Seleno.Samples.MvcMusicStore", 12345);
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            Browser.Close();
        }

        public HomePage HomePage
        {
            get
            {
                return new HomePage();
            }
        }
    }
}