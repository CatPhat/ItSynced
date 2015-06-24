using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var systemDirectories = new Crawler().Get(path);
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
            foreach (var directory in directories)
            {
                directoriesToReturn.Add(new Directory
                {
                    Directories = Get(directory.FullName),
                    DirectoryName = directory.Name,
                    Files = directory.GetFiles().Select(x => new File
                    {
                        FileName = x.Name,
                        LastModifiedDateTime = x.LastAccessTime
                    }).ToList()
                });
            }

            return directoriesToReturn;
        }
    }
}