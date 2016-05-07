using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Holf.AllForOne;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    internal class WebDriverBuilder<T> where T : IWebDriver
    {
        private readonly Func<IWebDriver> _factory;
        private string[] _processNames;
        private string _fileName;

        public WebDriverBuilder(Func<IWebDriver> factory)
        {
            _factory = factory;
        }

        public WebDriverBuilder<T> WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public WebDriverBuilder<T> WithProcessNames(params string[] processNames)
        {
            _processNames = processNames;
            return this;
        }

        private T Build()
        {
            if (_fileName != null)
            {
                EnsureFileExists(_fileName);
            }
            return CreateWebDriver();
        }

        public static implicit operator T(WebDriverBuilder<T> builder)
        {
            return builder.Build();
        }

        private static void EnsureFileExists(string resourceFileName)
        {
            // Already been loaded before?
            if (File.Exists(resourceFileName))
                return;

            // Find any assembly with the desired executable embedded in it
            // http://bloggingabout.net/blogs/vagif/archive/2010/07/02/net-4-0-and-notsupportedexception-complaining-about-dynamic-assemblies.aspx
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !(a is AssemblyBuilder))
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

        private T CreateWebDriver()
        {
            var processNames = _processNames ?? new [] {_fileName.Replace(@".exe", "")};

            var pidsBefore = processNames
                .SelectMany(Process.GetProcessesByName)
                .Select(p => p.Id)
                .ToArray();

            IWebDriver driver;
            try
            {
                driver = _factory();
            }
            finally
            {
                var pidsAfter = processNames
                    .SelectMany(Process.GetProcessesByName)
                    .Select(p => p.Id)
                    .ToArray();

                var newPids = pidsAfter.Except(pidsBefore);
                foreach (var pid in newPids)
                    Process.GetProcessById(pid).TieLifecycleToParentProcess();
            }

            return (T)driver;
        }

    }
}