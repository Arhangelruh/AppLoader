using System;
using System.IO;
using System.Linq;

namespace AppLoaderNet.Services
{
    public class FilesCopy
    {
        public int value = 0; 
        public void CopyFileAsync(IProgress<int> progress, string targetPath, string currenDirectory)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);

                CopyDirectory(currenDirectory, targetPath, progress);
            }
            else
            {
                CopyFiles(currenDirectory, targetPath, progress);
            }
        }

        public int GetCountFiles(string dir)
        {
            if (!Directory.Exists(dir))
            {
                throw new DirectoryNotFoundException("Directory not found");
            }

            var files = from file in Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories)
                        select file;

            return files.Count();
        }


        public void CopyDirectory(string sourceDir, string destinationDir, IProgress<int> progress)
        {
            bool recursive = true;

            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                                
                file.CopyTo(targetFilePath);
                value ++;                 
                    progress.Report(value);                
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, progress);
                }
            }
        }

        public void CopyFiles(string sourceDir, string destinationDir, IProgress<int> progress)
        {
            bool recursive = true;
            
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If destination directory not found, create new one.
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);

                if (File.Exists(targetFilePath))
                {
                    FileInfo targetFile = new FileInfo(targetFilePath);
                    if (file.LastWriteTime != targetFile.LastWriteTime)
                    {
                        file.CopyTo(targetFilePath, true);
                    }
                }
                else
                {
                    file.CopyTo(targetFilePath);
                }
                value++;
                progress.Report(value);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyFiles(subDir.FullName, newDestinationDir, progress);
                }
            }
        }
    }
}
