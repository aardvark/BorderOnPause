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
            const string releaseDirectoryPath = "../../Release";

            if (Directory.Exists(releaseDirectoryPath))
            {
                Console.WriteLine("Existing release bundle found. Removing...");
                Directory.Delete(releaseDirectoryPath, true);
            }

            var releaseDirectoryInfo = Directory.CreateDirectory(releaseDirectoryPath);

            // copy assemblies that have harmony bound
            DirectoryCopy("../../1.0", Path.Combine(releaseDirectoryPath, "1.0"), true);
            DirectoryCopy("../../1.1", Path.Combine(releaseDirectoryPath, "1.1"), true);

            // copy all from ../../1.2 excluding harmony stuff
            var excluding = new List<string> { "0Harmony.dll", "0Harmony.xml" };
            var versionsWithoutHarmony = new string[] { "1.2", "1.3", "1.4", "1.5" };
            foreach (var v in versionsWithoutHarmony)
            {
                Directory.CreateDirectory(releaseDirectoryPath +"/" + v + "/Assemblies");
                File.Copy(
                    "../../" + v + "/Assemblies/BorderOnPause.dll", 
                    releaseDirectoryPath + "/" + v + "/Assemblies/BorderOnPause.dll" 
                    );
            }

            // copy About, LoadFolders.xml, LICENSE
            DirectoryCopy("../../About",
                Path.Combine(releaseDirectoryPath, "About"),
                true);

            File.Copy("../../LoadFolders.xml", "../../Release/LoadFolders.xml");
            File.Copy("../../LICENSE", "../../Release/LICENSE");

            // update release in local Steam dir
            var modDirectoryPath = "C:/Program Files (x86)/Steam/steamapps/common/RimWorld/Mods/MoreVisiblePause";
            if (Directory.Exists(modDirectoryPath))
            {
                Console.WriteLine("Found existing mod directory. Cleaning...");
                Directory.Delete(modDirectoryPath, true);
            }
            else
            {
                Directory.CreateDirectory(modDirectoryPath);
            }
            
            
            Console.WriteLine("Copying release to the mod folder");
            DirectoryCopy(releaseDirectoryPath, modDirectoryPath, true);
        }

        private static readonly List<string> Empty = [];

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