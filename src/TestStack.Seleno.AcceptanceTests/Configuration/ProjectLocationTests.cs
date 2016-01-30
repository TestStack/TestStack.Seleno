﻿using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    [TestFixture]
    public class ProjectLocationTests
    {
        private string _rootPath;
        private string _webPath;
        private string _solutionFile;
        private readonly string _webAppName = "WebApp";
        private readonly string _originalCurrentDirectory = Environment.CurrentDirectory;

        private void DeleteDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directory.Delete(true);
            }
        }

        [SetUp]
        public void Setup()
        {
            _rootPath = Path.GetTempPath() + @"Seleno";
            _webPath = _rootPath + @"\WebApp\bin";
            _solutionFile = _rootPath + @"\Test.sln";

            Directory.CreateDirectory(_webPath);
            File.Create(_solutionFile).Close();

            Directory.SetCurrentDirectory(_webPath);

            ProjectLocation.SearchPaths = new[]
            {
                Environment.CurrentDirectory
            };
        }

        [TearDown]
        public void TearDown()
        {
            // Change directory back, so that _rootPath is no longer locked by our process.
            Directory.SetCurrentDirectory(_originalCurrentDirectory);

            File.Delete(_solutionFile);

            DeleteDirectory(_rootPath);
        }

        [Test]
        public void FromFolder_should_return_the_folder_of_our_web_application()
        {
            var solutionLocation = ProjectLocation.FromFolder(_webAppName);

            solutionLocation.FullPath.ShouldBeEquivalentTo(_rootPath + @"\" + _webAppName);
        }

        [Test]
        [ExpectedException(typeof(SolutionFileNotFoundException))]
        public void FromFile_should_raise_an_SolutionNotFoundException_when_no_solution_is_found()
        {
            File.Delete(_solutionFile);

            ProjectLocation.FromFolder(_webAppName);
        }

    }
}