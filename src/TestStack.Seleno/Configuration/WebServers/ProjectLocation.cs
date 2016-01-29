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

        private ProjectLocation(string fullPath)
        {
            var folder = new DirectoryInfo(fullPath);
            if (!folder.Exists)
            {
                throw new DirectoryNotFoundException();
            }
            FullPath = fullPath;
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

        private static string GetSolutionFolderPath(string basePath = null)
        {
            var baseDir = basePath ?? Environment.CurrentDirectory;

            var directory = new DirectoryInfo(baseDir);

            while (directory != null && directory.GetFiles("*.sln").Length == 0)
            {
                directory = directory.Parent;
            }

            return directory == null ? GetSolutionFolderPath(AppDomain.CurrentDomain.BaseDirectory) : directory.FullName;
        }

        private static string FindSubFolderPath(string rootFolderPath, string folderName)
        {
            if (string.IsNullOrEmpty(rootFolderPath))
            {
                throw new DirectoryNotFoundException();
            }

            var directory = new DirectoryInfo(rootFolderPath);

            directory = (directory.GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => string.Equals(folder.Name, folderName, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();

            if (directory == null)
            {
                throw new DirectoryNotFoundException();
            }

            return directory.FullName;
        }
    }
}