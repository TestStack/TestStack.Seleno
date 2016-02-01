using System;
using System.IO;
using System.Linq;

namespace TestStack.Seleno.Configuration.WebServers
{
    /// <inheritdoc />
    public class ProjectLocation : IProjectLocation
    {
        private static string[] _searchPaths;

        /// <inheritdoc />
        public string FullPath { get; private set; }

        private ProjectLocation(string fullPath)
        {
            var folder = new DirectoryInfo(fullPath);
            if (!folder.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            FullPath = fullPath;

        }

        internal ProjectLocation(string[] test)
        {
            _searchPaths = test;
        }

        static ProjectLocation()
        {
            _searchPaths = new[]
            {
                Environment.CurrentDirectory,
                AppDomain.CurrentDomain.RelativeSearchPath,
                AppDomain.CurrentDomain.BaseDirectory
            };
        }

        /// <summary>
        /// Returns the project location from the absolute file path to the web project.
        /// </summary>
        /// <param name="webProjectFullPath">The web project full path.</param>
        /// <returns></returns>
        public static ProjectLocation FromPath(string webProjectFullPath)
        {
            return new ProjectLocation(webProjectFullPath);
        }

        /// <summary>
        /// Returns the project location from the folder name of the web project.
        /// </summary>
        /// <param name="webProjectFolderName">Name of the web project folder.</param>
        public static ProjectLocation FromFolder(string webProjectFolderName)
        {
            var solutionFolder = GetSolutionFolderPath();
            var projectPath = FindSubFolderPath(solutionFolder, webProjectFolderName);
            return new ProjectLocation(projectPath);
        }

        private static DirectoryInfo FindSolution(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            var directory = new DirectoryInfo(path);

            while (directory != null && directory.GetFiles("*.sln").Length == 0)
            {
                directory = directory.Parent;
            }

            return directory;
        }

        private static string GetSolutionFolderPath()
        {
            foreach (var solutionPath in _searchPaths.Select(FindSolution).Where(solutionPath => solutionPath != null))
            {
                return solutionPath.FullName;
            }

            throw new SelenoException("Could not locate applications solution file.");
        }

        private static string FindSubFolderPath(string rootFolderPath, string folderName)
        {
            if (string.IsNullOrEmpty(rootFolderPath))
            {
                throw new DirectoryNotFoundException(rootFolderPath);
            }

            var directory = new DirectoryInfo(rootFolderPath);

            directory = (directory.GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => string.Equals(folder.Name, folderName, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();

            if (directory == null)
            {
                throw new DirectoryNotFoundException(rootFolderPath);
            }

            return directory.FullName;
        }
    }
}