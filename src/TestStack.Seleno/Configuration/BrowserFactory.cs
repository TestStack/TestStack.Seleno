using System;
using System.ComponentModel;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

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
            var options = new ChromeOptions();
            // addresses issue: https://code.google.com/p/chromedriver/issues/detail?id=799
            options.AddArgument("test-type");
            return new WebDriverBuilder<ChromeDriver>(() => new ChromeDriver(options))
                .WithFileName("chromedriver.exe");
        }

        /// <summary>
        /// Returns an initialised Chrome Web Driver.
        /// </summary>
        /// <remarks>You need to have chromedriver.exe embedded into your assembly and have Chrome installed on the machine running the test</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised Chrome driver</returns>
        public static ChromeDriver Chrome(ChromeOptions options)
        {
            return new WebDriverBuilder<ChromeDriver>(() => new ChromeDriver(options ?? new ChromeOptions()))
                .WithFileName("chromedriver.exe");

        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox()
        {
            return new WebDriverBuilder<FirefoxDriver>(() => new FirefoxDriver())
                .WithProcessName("firefox");
        }

        /// <summary>
        /// Returns an initialised Firefox Web Driver.
        /// </summary>
        /// <remarks>You need to have Firefox installed on the machine running the test</remarks>
        /// <param name="firefoxDriverService">Firefix driver service instance</param>
        /// <returns>Initialised Firefox driver</returns>
        public static FirefoxDriver FireFox(FirefoxDriverService firefoxDriverService)
        {
            return new WebDriverBuilder<FirefoxDriver>(() => new FirefoxDriver(firefoxDriverService))
                .WithProcessName("firefox");
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
            var options = new InternetExplorerOptions { IntroduceInstabilityByIgnoringProtectedModeSettings = true };
            return new WebDriverBuilder<InternetExplorerDriver>(() => new InternetExplorerDriver(options))
                .WithFileName("IEDriverServer.exe");
        }

        /// <summary>
        /// Returns an initialised 64-bit IE Web Driver.
        /// </summary>
        /// <remarks>You need to have IEDriverServer_x64_2.28.0.exe embedded into your assembly</remarks>
        /// <param name="options">Options to configure the driver</param>
        /// <returns>Initialised IE driver</returns>
        public static InternetExplorerDriver InternetExplorer(InternetExplorerOptions options)
        {
            return new WebDriverBuilder<InternetExplorerDriver>(() => new InternetExplorerDriver(options))
                .WithFileName("IEDriverServer.exe");
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
            : base($"Could not find configured web driver; you need to embed an executable with the filename {expectedExecutableName}.")
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
            : base($"Could not find browser: {expectedBrowser}.", innerException)
        { }
    }
}
