using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Holf.AllForOne;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// Allows the creation of Browser specific web drivers.
    /// </summary>
    public static class BrowserFactory
    {
        /// <summary>
        /// Returns an initialised PhantomJS Web Driver.
        /// </summary>
        /// <remarks>You need to have phantomjs.exe embedded into your assembly</remarks>
        /// <returns>Initialised PhantomJS driver</returns>
        public static PhantomJSDriver PhantomJS()
        {
            CreateDriver("phantomjs.exe");
            var driver = new PhantomJSDriver();
            TieLifecycleToParentProcess("phantomjs");
            return driver;
        }

        /// <summary>
        /// Returns an initialised PhantomJS Web Driver.
        /// </summary>
        /// <remarks>You need to have phantomjs.exe embedded into your assembly</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised PhantomJS driver</returns>
        public static PhantomJSDriver PhantomJS(PhantomJSOptions options)
        {
            CreateDriver("phantomjs.exe");
            var driver = new PhantomJSDriver(options);
            TieLifecycleToParentProcess("phantomjs");
            return driver;
        }

        /// <summary>
        /// Returns an initialised Chrome Web Driver.
        /// </summary>
        /// <remarks>You need to have chromedriver.exe embedded into your assembly and have Chrome installed on the machine running the test</remarks>
        /// <returns>Initialised Chrome driver</returns>
        public static ChromeDriver Chrome()
        {
            CreateDriver("chromedriver.exe");
            var driver = new ChromeDriver();
            TieLifecycleToParentProcess("chromedriver");
            return driver;
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
            var driver = new ChromeDriver(options ?? new ChromeOptions());
            TieLifecycleToParentProcess("chromedriver");
            return driver;
        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox()
        {
            var driver = new FirefoxDriver();
            TieLifecycleToParentProcess("firefox");
            return driver;
        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <param name="profile">Profile to use for the driver</param>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox(FirefoxProfile profile)
        {
            var driver = new FirefoxDriver(profile);
            TieLifecycleToParentProcess("firefox");
            return driver;
        }

        /// <summary>
        /// Returns an initialised Safari Web Driver.
        /// </summary>
        /// <remarks>You need to have Safari installed on the machine running the test</remarks>
        /// <returns>Initialised Safari driver</returns>
        public static SafariDriver Safari()
        {
            try
            {
                return new SafariDriver();
            }
            catch (Win32Exception e)
            {
                throw new BrowserNotFoundException("Safari", e);
            }
        }

        /// <summary>
        /// Returns an initialised Safari Web Driver.
        /// </summary>
        /// <remarks>You need to have Safari installed on the machine running the test</remarks>
        /// <param name="options">Profile to use for the driver</param>
        /// <returns>Initialised Safari driver</returns>
        public static SafariDriver Safari(SafariOptions options)
        {
            try
            {
                return new SafariDriver(options);
            }
            catch (Win32Exception e)
            {
                throw new BrowserNotFoundException("Safari", e);
            }
        }

        /// <summary>
        /// Returns an initialised 64-bit IE Web Driver.
        /// </summary>
        /// <remarks>You need to have IEDriverServer_x64_2.28.0.exe embedded into your assembly</remarks>
        /// <returns>Initialised IE driver</returns>
        public static InternetExplorerDriver InternetExplorer()
        {
            CreateDriver("IEDriverServer.exe");
            var options = new InternetExplorerOptions { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            var driver = new InternetExplorerDriver(options);
            TieLifecycleToParentProcess("IEDriverServer");
            return driver;
        }
        /// <summary>
        /// Returns an initialised 64-bit IE Web Driver.
        /// </summary>
        /// <remarks>You need to have IEDriverServer_x64_2.28.0.exe embedded into your assembly</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised IE driver</returns>
        public static InternetExplorerDriver InternetExplorer(InternetExplorerOptions options)
        {
            CreateDriver("IEDriverServer.exe");
            var driver = new InternetExplorerDriver(options);
            TieLifecycleToParentProcess("IEDriverServer");
            return driver;
        }

        private static void CreateDriver(string resourceFileName)
        {
            // Already been loaded before?
            if (File.Exists(resourceFileName))
                return;

            // Find any assembly with the desired executable embedded in it
            // http://bloggingabout.net/blogs/vagif/archive/2010/07/02/net-4-0-and-notsupportedexception-complaining-about-dynamic-assemblies.aspx
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !(a is System.Reflection.Emit.AssemblyBuilder))
                .Where(a => a.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder")
                .Where(a => !a.GlobalAssemblyCache)
                .FirstOrDefault(a => a
                    .GetManifestResourceNames()
                    .Any(x => x.EndsWith(resourceFileName, true, CultureInfo.InvariantCulture))
                );

            if (assembly == null)
                throw new WebDriverNotFoundException(resourceFileName);

            // Write embedded resource to disk so Selenium Web Driver can use it
            var resourceName = assembly.GetManifestResourceNames().First(x => x.EndsWith(resourceFileName, true, CultureInfo.InvariantCulture));
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var fileStream = new FileStream(resourceFileName, FileMode.Create))
            {
                // ReSharper disable PossibleNullReferenceException
                resourceStream.CopyTo(fileStream);
                // ReSharper restore PossibleNullReferenceException
            }
        }

        private static void TieLifecycleToParentProcess(string processName)
        {
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process != null)
            {
                process.TieLifecycleToParentProcess();
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

    /// <summary>
    /// Exception to record inability to find a Browser.
    /// </summary>
    public class BrowserNotFoundException : SelenoException
    {
        /// <summary>
        /// Create a web driver not found exception for the given driver.
        /// </summary>
        /// <param name="expectedBrowser">The name of the expected browser</param>
        /// <param name="innerException">The exception that indicated the browser couldn't be found</param>
        public BrowserNotFoundException(string expectedBrowser, Exception innerException)
            : base(string.Format("Could not find browser: {0}.", expectedBrowser), innerException)
        { }
    }
}
