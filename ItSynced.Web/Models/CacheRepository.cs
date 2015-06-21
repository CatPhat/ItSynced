﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItSynced.Web.Helpers;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Expiration.Interfaces;

namespace ItSynced.Web.Models
{
   

    public class CacheRepository
    {
        private readonly MemoryCache _cache;

        public CacheRepository()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public object GetItem(string directory)
        {
            if (directory == null)
            {
                directory = "." + Path.DirectorySeparatorChar;
            }

            object cachedObject;
            if (_cache.TryGetValue(directory, out cachedObject))
            {
               return cachedObject;
            }

            _cache.Set(directory, context =>
            {
                context.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
               // context.RegisterPostEvictionCallback((echoKey, value, reason, substate) => { GetItem(directory); }, state: null);
                return new Lazy<object>(() => InitItem(directory)).Value;
            });

            return GetItem(directory);

        }

        private object InitItem(string directory)
        {
            return new DirectoryCrawler().GetFolderNames(directory);
        }
    }

    public class MemoryCacheContext
    {
        private readonly MemoryCache _cache;
        public MemoryCacheContext()
        {
            
        }

        public void SetPriority(CachePreservationPriority priority)
        {
            _context.SetPriority(priority);
        }

        public void SetAbsoluteExpiration(DateTimeOffset absolute)
        {
           _context.SetAbsoluteExpiration(absolute);
        }

        public void SetAbsoluteExpiration(TimeSpan relative)
        {
            _context.SetAbsoluteExpiration(relative);
        }

        public void SetSlidingExpiration(TimeSpan offset)
        {
            _context.SetSlidingExpiration(offset);
        }

        public void AddExpirationTrigger(IExpirationTrigger trigger)
        {
            _context.AddExpirationTrigger(trigger);
        }

        public void RegisterPostEvictionCallback(Action<string, object, EvictionReason, object> callback, object state)
        {
            _context.RegisterPostEvictionCallback(callback, state);
        }

        public string Key => _context.Key;

        public object State => _context.State;
    }

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