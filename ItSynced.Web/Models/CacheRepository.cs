using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItSynced.Web.Helpers;
using Microsoft.Framework.Caching.Memory;

namespace ItSynced.Web.Models
{
    public static class CacheRepository
    {
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static object GetItem(string key)
        {
            return _cache.GetOrSet(key, context =>
            {
                context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                return InitItem(key);
            });
        }

        private static object InitItem(string key)
        {
            return new DirectoryCrawler().GetFolderNames(key);
        }
    }

    public class DirectoryCrawler
    {
        public SyncedFilesViewModel GetFolderNames(string path)
        {
            var directories = new GetAllDirectoryItems().Get(path).ToList();
            directories =
                directories.Flatten(x => x.Direcories)
                    .Where(j => j.Files.Any())
                    .OrderByDescending(y => y.Files.Max(z => z.LastModifiedTime))
                    .Take(20)
                    .ToList();


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
                        FullPath = directoryPath + "/" + dir.Name,
                        Direcories = Get(directoryPath + "/" + dir.Name)
                    }).ToList().OrderByDescending(x => x.LastModifiedTime);
                }
                return null;
            }
        }
    }
}