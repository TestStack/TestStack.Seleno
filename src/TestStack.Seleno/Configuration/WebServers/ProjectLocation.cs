using System;
using System.IO;
using System.Linq;

namespace TestStack.Seleno.Configuration.WebServers
{
    public interface IProjectLocation
    {
        string FullPath { get; }
    }

    public class ProjectLocation : IProjectLocation
    {
        public string FullPath { get; private set; }

        private static string[] searchPaths;

        private ProjectLocation(string fullPath)
        {
            var folder = new DirectoryInfo(fullPath);
            if (!folder.Exists)
            {
                throw new DirectoryNotFoundException();
            }

            FullPath = fullPath;

        }

        public ProjectLocation(string[] test)
        {
            searchPaths = test;
        }

        static ProjectLocation()
        {
            searchPaths = new[]
            {
                Environment.CurrentDirectory,
                AppDomain.CurrentDomain.RelativeSearchPath,
                AppDomain.CurrentDomain.BaseDirectory
            };
        }

        public static ProjectLocation FromPath(string webProjectFullPath)
        {
            return new ProjectLocation(webProjectFullPath);
        }

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
            foreach (var solutionPath in searchPaths.Select(FindSolution).Where(solutionPath => solutionPath != null))
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