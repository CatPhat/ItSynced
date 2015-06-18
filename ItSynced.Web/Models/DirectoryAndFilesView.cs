using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItSynced.Web.Models
{
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
}
