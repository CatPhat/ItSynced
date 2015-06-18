using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ItSynced.Web.Helpers;
using Microsoft.Framework.Caching;
using Microsoft.Framework.Caching.Memory;

namespace ItSynced.Web.Models
{
    public static class CacheRepository
    {
        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static object GetItem(string key)
        {
            return GetOrAddExisting(key, () => InitItem(key));
        }

        private static T GetOrAddExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
         
            Lazy<T> oldValue;
            _cache.TryGetValue(key, out oldValue);
            try
            {
                _cache.Set(key,(oldValue ?? newValue).Value, new MemoryCacheEntryOptions());
                return (T) _cache.Get(key);
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache. Thanks to Denis Borovnev for pointing this out!
                _cache.Remove(key);
                throw;
            }
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
                    .Take(40)
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
                        FullPath = directoryPath + "\\" + dir.Name,
                        Direcories = Get(directoryPath + "\\" + dir.Name)
                    }).ToList().OrderByDescending(x => x.LastModifiedTime);
                }
                return null;
            }
        }
    }
}