using System;
using System.IO;
using System.Linq;

namespace TestStack.Seleno.Configuration.WebServers
{
    public class ProjectLocation
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
            string solutionFolder = GetSolutionFolderPath();
            string projectPath = FindSubFolderPath(solutionFolder, webProjectFolderName);
            return new ProjectLocation(projectPath);
        }

        private static string GetSolutionFolderPath()
        {
            var directory = new DirectoryInfo(Environment.CurrentDirectory);

            while (directory.GetFiles("*.sln").Length == 0)
            {
                directory = directory.Parent;
            }
            return directory.FullName;
        }

        private static string FindSubFolderPath(string rootFolderPath, string folderName)
        {
            var directory = new DirectoryInfo(rootFolderPath);

            directory = (directory.GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => folder.Name.ToLower() == folderName.ToLower()))
                .FirstOrDefault();

            if (directory == null)
            {
                throw new DirectoryNotFoundException();
            }

            return directory.FullName;
        }
    }
}