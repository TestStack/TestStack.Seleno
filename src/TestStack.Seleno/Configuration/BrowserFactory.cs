using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Configuration
{
    public static class BrowserFactory
    {
        public static ChromeDriver Chrome()
        {
            CreateDriver("chromedriver.exe");
            return new ChromeDriver();
        }

        public static FirefoxDriver FireFox()
        {
            var browser = new FirefoxDriver();
            browser.SetImplicitTimeout(10);
            return browser;
        }

        public static InternetExplorerDriver InternetExplorer32(InternetExplorerOptions options = null)
        {
            CreateDriver("IEDriverServer_Win32_2.28.0.exe", "IEDriverServer.exe");
            return InternetExplorer(options);
        }

        public static InternetExplorerDriver InternetExplorer64(InternetExplorerOptions options = null)
        {
            CreateDriver("IEDriverServer_x64_2.28.0.exe", "IEDriverServer.exe");
            return InternetExplorer(options);
        }

        private static InternetExplorerDriver InternetExplorer(InternetExplorerOptions options = null)
        {
            if (options == null)
            {
                options = new InternetExplorerOptions();
                options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
            }
            return new InternetExplorerDriver(options);
        }

        private static void CreateDriver(string resourceFileName, string outputName = null)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith(resourceFileName));

            if (!File.Exists(resourceFileName))
            {
                var resourceStream = assembly.GetManifestResourceStream(resourceName);
                var resourceBytes = new byte[(int)resourceStream.Length];

                resourceStream.Read(resourceBytes, 0, resourceBytes.Length);
                if (outputName == null)
                    outputName = resourceFileName;
                File.WriteAllBytes(outputName, resourceBytes);
            }
        }
    }
}