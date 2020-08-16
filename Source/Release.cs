using System;
using System.Collections.Generic;
using System.IO;

namespace BorderOnPause
{
    public class Release
    {
        public static void Main()
        {
            Console.WriteLine("Creating release bundle for mod.");
            var releaseDirectoryPath = "../../Release";
            var releaseDirectoryInfo = Directory.CreateDirectory(releaseDirectoryPath);
            
            // copy assemblies
            // copy all from ../../1.0
            DirectoryCopy("../../1.0", Path.Combine(releaseDirectoryPath, "1.0"), true);
            // copy all from ../../1.1
            DirectoryCopy("../../1.1", Path.Combine(releaseDirectoryPath, "1.1"), true);

            // copy all from ../../1.2 excluding harmony stuff
            var excluding = new List<string> {"0Harmony.dll", "0Harmony.xml"};
            DirectoryCopy("../../1.2", Path.Combine(releaseDirectoryPath, "1.2"), true, excluding);

            // copy About
            DirectoryCopy("../../About",
                Path.Combine(releaseDirectoryPath, "About"),
                true);

            // copy LoadFolders.xml
            File.Copy("../../LoadFolders.xml", "../../Release/LoadFolders.xml");
            // copy LICENSE
            File.Copy("../../LICENSE", "../../Release/LICENSE");
        }

        private static readonly List<string> Empty = new List<string>();

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryCopy(sourceDirName, destDirName, copySubDirs, Empty);
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs,
            List<string> excluding)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = Array.FindAll(dir.GetFiles(), info => !excluding.Contains(info.Name));
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (!copySubDirs) return;
            
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, true, excluding);
            }
        }
    }
}