using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItSynced.Web.Helpers;
using ItSynced.Web.Models;

namespace ItSynced.Web.DAL.LocalsystemCrawler
{
    public class DirectoryCrawler
    {
        public SyncedFilesViewModel GetFolderNames(string path)
        {
            var directories = new GetAllDirectoryItems().Get(path).Flatten(x => x.Direcories)
                .Where(j => j.Files.Any())
                .OrderByDescending(y => y.Files.Max(z => z.LastModifiedTime))
                .Take(20).ToList();

            var model = new SyncedFilesViewModel {Folders = directories};
            return model;
        }

        public class GetAllDirectoryItems
        {
            public IEnumerable<DirectoryAndFilesView> Get(string directoryPath)
            {
                var directories = new DirectoryInfo(directoryPath).GetDirectories();
                if (directories.Any())
                {
                    return directories.Select(dir => new DirectoryAndFilesView
                    {
                        Files = dir.GetFiles().Select(file => new FileView
                        {
                            FileName = file.Name,
                            FullPath = file.DirectoryName,
                            LastModifiedTime = file.LastAccessTime,
                            ParentFolderName = file.Directory.Name
                        }).ToList().OrderByDescending(thisFile => thisFile.LastModifiedTime).Take(10),
                        DirectoryName = dir.Name,
                        FullPath = Path.Combine(directoryPath, dir.Name),
                        Direcories = Get(Path.Combine(directoryPath, dir.Name))
                    }).ToList().OrderByDescending(x => x.LastModifiedTime);
                }
                return null;
            }
        }
    }
}