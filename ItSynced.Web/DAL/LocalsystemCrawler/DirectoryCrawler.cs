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
            var directories = new GetAllDirectoryItems().Get(path).Flatten(x => x.Directories)
                .Where(j => j.Files.Any())
                .OrderByDescending(y => y.Files.Max(z => z.LastModifiedDateTime))
                .Take(20).ToList();
            return directories;
        }

        public class GetAllDirectoryItems
        {
            public List<Directory> Get(string directoryPath)
            {
                var directories = new DirectoryInfo(directoryPath).GetDirectories();
                if (directories.Any())
                {
                    return directories.Select(dir => new Directory
                    {
                        Files = dir.GetFiles().Select(file => new File
                        {
                            FileName = file.Name,
                            LastModifiedDateTime = file.LastAccessTime,
                            ParentDirectoryName = file.Directory.Name
                        }).ToList().OrderByDescending(thisFile => thisFile.LastModifiedDateTime).Take(10),
                        DirectoryName = dir.Name,
                        FullPath = Path.Combine(directoryPath, dir.Name),
                        Directories = Get(Path.Combine(directoryPath, dir.Name))
                    }).OrderByDescending(x => x.LastModifiedDateTime).ToList();
                }
                return null;
            }
        }
    }
}