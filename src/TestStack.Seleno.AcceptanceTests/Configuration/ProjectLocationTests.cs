using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    [TestFixture]
    public class ProjectLocationTests
    {
        private string _rootPath;
        private string _webPath;
        private string _webApplicationPath;
        private string _solutionFile;
        private readonly string _webAppName = "WebApp";
        private string _originalCurrentDirectory;

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
            _webApplicationPath = _rootPath + @"\WebApp";
            _originalCurrentDirectory = Environment.CurrentDirectory;

            Directory.CreateDirectory(_webPath);
            File.Create(_solutionFile).Close();

            Directory.SetCurrentDirectory(_webPath);

            new ProjectLocation(new[]
            {
                Environment.CurrentDirectory
            });

        }

        [TearDown]
        public void TearDown()
        {
            // Change directory back, so that _rootPath is no longer locked by our process.
            Directory.SetCurrentDirectory(_originalCurrentDirectory);
            File.Delete(_solutionFile);
            DeleteDirectory(_rootPath);
            new ProjectLocation(new[]
            {
                Environment.CurrentDirectory,
                AppDomain.CurrentDomain.RelativeSearchPath,
                AppDomain.CurrentDomain.BaseDirectory
            });
        }

        [Test]
        public void FromFolder_should_return_the_folder_of_web_application()
        {
            var solutionLocation = ProjectLocation.FromFolder(_webAppName);
            solutionLocation.FullPath.ShouldBeEquivalentTo(_rootPath + @"\" + _webAppName);
        }

        [Test]
        public void FromFolder_should_raise_a_SolutionNotFoundException_when_no_solution_is_found()
        {
            File.Delete(_solutionFile);
            Action action = () => ProjectLocation.FromFolder(_webAppName);
            action.ShouldThrow<SelenoException>();
        }

        [Test]
        public void FromFolder_should_raise_a_DirectoryNotFoundException_when_no_folder_is_found()
        {
            Action action = () => ProjectLocation.FromFolder("non-existent-folder");
            action.ShouldThrow<DirectoryNotFoundException>()
                .WithMessage(_rootPath);
        }

        [Test]
        public void FromPath_should_return_the_path_to_web_application()
        {
            var solutionLocation = ProjectLocation.FromPath(_webApplicationPath);
            solutionLocation.FullPath.ShouldBeEquivalentTo(_webApplicationPath);
        }
    }
}