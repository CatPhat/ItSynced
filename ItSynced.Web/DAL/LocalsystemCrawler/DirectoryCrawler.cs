using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ItSynced.Web.Helpers;
using Directory = ItSynced.Web.DAL.Entities.Directory;
using File = ItSynced.Web.DAL.Entities.File;

namespace ItSynced.Web.DAL.LocalsystemCrawler
{
    public class DirectoryCrawler
    {
        public List<Directory> GetDirectories(string path)
        {
           
            var directories = new List<Directory>();
            var systemDirectories = new Crawler().GetAndSetParentDirectory(path, new Directory
            {
                DirectoryName = "ROOT",
                FullPath = ""
            });
            foreach (var directory in systemDirectories.Flatten(x => x.Directories))
            {
                directories.Add(directory);
            }
            return directories;
        }

      
    }

    public class Crawler
    {
        public List<Directory> Get(string directoryPath)
        {
            var directoriesToReturn = new List<Directory>();
            var directories = new DirectoryInfo(directoryPath).GetDirectories();
            foreach (var systemDirectory in directories)
            {
                var newDirectory = new Directory
                {
                    DirectoryName = systemDirectory.Name,
                    FullPath = systemDirectory.FullName,
                    Files = systemDirectory.GetFiles().Select(x => new File
                    {
                        FileName = x.Name,
                        LastModifiedDateTime = x.LastAccessTime,
                        FullPath = x.FullName

                    }).ToList()
                };
                newDirectory.Directories = GetAndSetParentDirectory(newDirectory.FullPath, newDirectory);
                directoriesToReturn.Add(newDirectory);
            }

            return directoriesToReturn;
        }

        public List<Directory> GetAndSetParentDirectory(string directoryPath, Directory parentDirectory)
        {
            var directories = Get(directoryPath);
            foreach (var directory in directories)
            {
                directory.ParentDirectory = parentDirectory;
            }

            return directories;
            
        }
    }
}