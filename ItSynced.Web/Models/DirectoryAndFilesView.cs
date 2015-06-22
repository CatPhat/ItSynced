using System;
using System.Collections.Generic;
using System.Linq;
using ItSynced.Web.DAL.Entities;

namespace ItSynced.Web.Models
{
    public class DirectoriesAndFilesView
    {
        public string DirectoryName { get; set; }
        public string LastModifiedTime { get; set; }
        public string ParentFolderName { get; set; }
        public int ItemCount { get; set; }
        public string FullPath { get; set; }
        public string ParentDirectory { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Directory> Directories { get; set; }

        public DateTime RecentModifiedFileTime
        {
            get
            {
                return Files.OrderByDescending(y => y.LastModifiedDateTime).First().LastModifiedDateTime;
            }
        }

        public TimeSpan TimeElapsedSinceLastModifiedFile => DateTime.Now - RecentModifiedFileTime;
    }
}
