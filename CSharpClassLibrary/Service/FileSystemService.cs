using System;
using System.IO;
using System.Security.Permissions;

namespace CSharpClassLibrary.Service
{
    static class FileSystemService
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void RunFileSystemWatcher(string path)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher()) {
                watcher.Path = path;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e) =>
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

        private static void OnRenamed(object source, RenamedEventArgs e) =>
        Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}