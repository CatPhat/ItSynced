using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace ItSynced.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var model = GetFolderNames();
          
            return View(model);
        }

        [HttpGet]
        public SyncedFilesViewModel GetFolderNames()
        {
            const string location = @"M:\The_Armory\Allegro\Libraries\ltc\wdslibs";

            if (HttpRuntime.Cache.Get("files") == null)
            {
                var directories = new GetAllDirectoryItems().Get(location).ToList();
                directories = directories.Flatten(x => x.Direcories).Where(j => j.Files.Any()).OrderByDescending(y => y.Files.Max(z => z.LastModifiedTime)).Take(40).ToList();
                HttpRuntime.Cache.Insert("files",directories, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10));
            }
            var model = new SyncedFilesViewModel { Folders =  (IEnumerable<DirectoryAndFilesView>)HttpRuntime.Cache.Get("files") };
            return  model;
        }
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
                        FileName =  file.Name,
                        FullPath = file.DirectoryName,
                        LastModifiedTime = file.LastAccessTime,
                        ParentFolderName = file.Directory.Name,

                    }).ToList().OrderByDescending(thisFile => thisFile.LastModifiedTime).Take(10),
                    DirectoryName = dir.Name,
                    FullPath = directoryPath + "\\" + dir.Name,
                    Direcories = Get(directoryPath + "\\" + dir.Name),
                   
                }).ToList().OrderByDescending(x => x.LastModifiedTime);
            }
            return null;
        } 
    }

    public static class Helper
    {
        // Depth-first traversal, recursive
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector)
        {
            if (source == null) yield break;
            foreach (var item in source)
            {
                yield return item;
                foreach (var child in childrenSelector(item).Flatten(childrenSelector))
                {
                    yield return child;
                }
            }
        }
    }

    public class SyncedFilesViewModel
    {
        public IEnumerable<DirectoryAndFilesView> Folders { get; set; }
    }

  

    public class DirectoryAndFilesView
    {
        public string DirectoryName { get; set; }
        public string LastModifiedTime { get; set; }
        public string ParentFolderName { get; set; }
        public int ItemCount { get; set; }
        public string FullPath { get; set; }
        public string ParentDirectory { get; set; }
        public IEnumerable<FileView> Files { get; set; }
        public IEnumerable<DirectoryAndFilesView> Direcories { get; set; }

        public DateTime RecentModifiedFileTime
        {
            get
            {
              return Files.OrderByDescending(y => y.LastModifiedTime).First().LastModifiedTime;
            } 
        }

        public TimeSpan TimeElapsedSinceLastModifiedFile => DateTime.Now - RecentModifiedFileTime;
    }

    public class FileView
    {
        public string ParentFolderName { get; set; }
        public string FileName { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public string FullPath { get; set; }
        
    }
}