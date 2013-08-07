using System;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// Allows the creation of Browser specific web drivers.
    /// </summary>
    public static class BrowserFactory
    {
        /// <summary>
        /// Returns an initialised Chrome Web Driver.
        /// </summary>
        /// <remarks>You need to have chromedriver.exe embedded into your assembly and have Chrome installed on the machine running the test</remarks>
        /// <returns>Initialised Chrome driver</returns>
        public static ChromeDriver Chrome()
        {
            var a = Assembly.GetExecutingAssembly();
            CreateDriver("chromedriver.exe");
            return new ChromeDriver();
        }

        /// <summary>
        /// Returns an initialised Chrome Web Driver.
        /// </summary>
        /// <remarks>You need to have chromedriver.exe embedded into your assembly and have Chrome installed on the machine running the test</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised Chrome driver</returns>
        public static ChromeDriver Chrome(ChromeOptions options)
        {
            CreateDriver("chromedriver.exe");
            return new ChromeDriver(options ?? new ChromeOptions());
        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox()
        {
            return new FirefoxDriver();
        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <param name="profile">Profile to use for the driver</param>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox(FirefoxProfile profile)
        {
            return new FirefoxDriver(profile);
        }

        /// <summary>
        /// Returns an initialised 32-bit IE Web Driver.
        /// </summary>
        /// <remarks>You need to have IEDriverServer_Win32_2.28.0.exe embedded into your assembly</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised IE driver</returns>
        public static InternetExplorerDriver InternetExplorer32(InternetExplorerOptions options = null)
        {
            CreateDriver("IEDriverServer_Win32_2.28.0.exe", "IEDriverServer.exe");
            return InternetExplorer(options);
        }

        /// <summary>
        /// Returns an initialised 64-bit IE Web Driver.
        /// </summary>
        /// <remarks>You need to have IEDriverServer_x64_2.28.0.exe embedded into your assembly</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised IE driver</returns>
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
            outputName = outputName ?? resourceFileName;

            // Already been loaded before?
            if (File.Exists(outputName))
                return;

            // Find any assembly with the desired executable embedded in it
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetManifestResourceNames().Any())
                .FirstOrDefault(a => a
                    .GetManifestResourceNames()
                    .Any(x => x.EndsWith(resourceFileName))
                );

            if (assembly == null)
                throw new WebDriverNotFoundException(resourceFileName);

            // Write embedded resource to disk so Selenium Web Driver can use it
            var resourceName = assembly.GetManifestResourceNames().First(x => x.EndsWith(resourceFileName));
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = new FileStream(outputName, FileMode.Create))
            {
                resourceStream.CopyTo(fileStream);
            }
        }
    }

    /// <summary>
    /// Exception to record inability to find a Web Driver.
    /// </summary>
    public class WebDriverNotFoundException : SelenoException
    {
        /// <summary>
        /// Create a web driver not found exception for the given driver.
        /// </summary>
        /// <param name="expectedExecutableName">The name of the expected executable to be embedded in the assembly</param>
        public WebDriverNotFoundException(string expectedExecutableName)
            : base(string.Format("Could not find configured web driver; you need to embed an executable with the filename {0}.",
                expectedExecutableName
            ))
        {}
    }
}
