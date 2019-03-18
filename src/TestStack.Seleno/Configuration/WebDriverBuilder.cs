using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Holf.AllForOne;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    internal class WebDriverBuilder<T> where T : IWebDriver
    {
        private readonly Func<IWebDriver> _factory;
        private string _fileName;
        private string _processName;

        public WebDriverBuilder(Func<IWebDriver> factory)
        {
            _factory = factory;
        }

        public static implicit operator T(WebDriverBuilder<T> builder)
        {
            return builder.Build();
        }

        public WebDriverBuilder<T> WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public WebDriverBuilder<T> WithProcessName(string processName)
        {
            _processName = processName;
            return this;
        }

        private static void EnsureFileExists(string resourceFileName)
        {
            // Already been loaded before?
            if (File.Exists(resourceFileName))
            {
                return;
            }

            var fileFound = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .EnumerateFiles()
                .Any(f => f.Name == resourceFileName);

            if (fileFound == false)
            {
                throw new WebDriverNotFoundException(resourceFileName);
            }
        }

        private T Build()
        {
            if (_fileName != null)
            {
                EnsureFileExists(_fileName);
            }

            return CreateWebDriver();
        }

        private T CreateWebDriver()
        {
            var processName = _processName ?? _fileName.Replace(@".exe", "");

            var pidsBefore = Process
                .GetProcessesByName(processName)
                .Select(p => p.Id);

            var driver = _factory();

            var pidsAfter = Process
                .GetProcessesByName(processName)
                .Select(p => p.Id);

            var newPids = pidsAfter.Except(pidsBefore);
            foreach (var pid in newPids)
            {
                Process.GetProcessById(pid).TieLifecycleToParentProcess();
            }

            return (T) driver;
        }
    }
}